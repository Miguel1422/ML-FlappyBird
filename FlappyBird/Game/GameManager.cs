using FlappyBird.Visual.Game;
using FlappyBird.Visual.NN;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Game
{
    public class GameManager
    {

        public static int WIDTH = 800;
        public static int HEIGHT = 600;


        private static Random random;

        static GameManager()
        {
            random = Constants.r;
        }
        int totalPopulation = 500;
        // All active birds (not yet collided with pipe)
        List<Bird> activeBirds = new List<Bird>();
        // All birds for any given population
        List<Bird> allBirds = new List<Bird>();
        // Pipes
        List<Pipe> pipes = new List<Pipe>();
        // A frame counter to determine when to add a pipe
        int counter = 0;
        public Bird BestBird { get; private set; }
        // Interface elements
        int speedSlider;
        int speedSpan;
        int highScoreSpan;
        int allTimeHighScoreSpan;

        // All time high score
        int highScore = 0;



        public GameManager()
        {

            for (int i = 0; i < totalPopulation; i++)
            {
                var bird = new Bird();
                activeBirds.Add(bird);
                allBirds.Add(bird);
            }
        }

        public int cycles = 1;

        public void draw(Graphics canvas)
        {
            canvas.Clear(Color.White);

            // Should we speed up cycles per frame
            // How many times to advance the game
            for (int n = 0; n < cycles; n++)
            {
                // Show all the pipes
                for (int i = pipes.Count - 1; i >= 0; i--)
                {
                    pipes[i].update();
                    if (pipes[i].offscreen())
                    {
                        pipes.RemoveAt(i);
                    }
                }

                for (int i = activeBirds.Count - 1; i >= 0; i--)
                {
                    var bird = activeBirds[i];
                    // Bird uses its brain!
                    bird.Think(pipes);
                    bird.update();

                    if (bird.bottomTop())
                    {
                        activeBirds.RemoveAt(i);
                        continue;
                    }

                    // Check all the pipes
                    for (int j = 0; j < pipes.Count; j++)
                    {
                        // It's hit a pipe
                        if (pipes[j].hits(activeBirds[i]))
                        {
                            // Remove this bird
                            activeBirds.RemoveAt(i);
                            break;
                        }
                    }



                }


                // Add a new pipe every so often
                if (counter % 75 == 0)
                {
                    pipes.Add(new Pipe());
                }
                counter++;
            }

            // What is highest score of the current population
            int tempHighScore = 0;
            // If we're training

            // Which is the best bird?
            Bird tempBestBird = null;
            for (int i = 0; i < activeBirds.Count; i++)
            {
                var s = activeBirds[i].score;
                if (s > tempHighScore)
                {
                    tempHighScore = s;
                    tempBestBird = activeBirds[i];
                }
            }


            // Is it the all time high scorer?
            if (tempHighScore > highScore)
            {
                highScore = tempHighScore;
                this.BestBird = tempBestBird;
            }



            // Draw everything!
            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].show(canvas);
            }

            for (int i = 0; i < activeBirds.Count; i++)
            {
                activeBirds[i].show(canvas);
            }
            // If we're out of birds go to the next generation
            if (activeBirds.Count == 0)
            {
                nextGeneration();
            }
        }



        // Start the game over
        public void resetGame()
        {
            counter = 0;
            pipes.Clear();
        }

        // Create the next generation
        private void nextGeneration()
        {
            resetGame();
            // Normalize the fitness values 0-1
            normalizeFitness(allBirds);
            // Generate a new set of birds
            activeBirds = generate(allBirds);
            // Copy those birds to another array
            allBirds = new List<Bird>(activeBirds);
        }

        public void saveBestBird()
        {
            string bestBird = JsonConvert.SerializeObject(BestBird.brain, Formatting.Indented);
            System.IO.File.WriteAllText("bird.txt", bestBird);
        }

        public void loadBestBird()
        {
            string bestBird = System.IO.File.ReadAllText("bird.txt");
            NeuralNetwork nn = JsonConvert.DeserializeObject<NeuralNetwork>(bestBird);

            activeBirds.Clear();
            allBirds.Clear();
            for (int i = 0; i < totalPopulation; i++)
            {
                var bird = new Bird(nn);
                activeBirds.Add(bird);
                allBirds.Add(bird);
            }

            nextGeneration();

        }

        // Generate a new population of birds
        private List<Bird> generate(List<Bird> oldBirds)
        {
            List<Bird> newBirds = new List<Bird>();
            for (int i = 0; i < oldBirds.Count; i++)
            {
                // Select a bird based on fitness
                var bird = poolSelection(oldBirds);
                newBirds.Add(bird);
            }
            return newBirds;
        }

        // Normalize the fitness of all birds
        private void normalizeFitness(List<Bird> birds)
        {
            // Make score exponentially better?
            for (int i = 0; i < birds.Count; i++)
            {
                birds[i].score = birds[i].score * birds[i].score;
            }

            // Add up all the scores
            int sum = 0;
            for (int i = 0; i < birds.Count; i++)
            {
                sum += birds[i].score;
            }
            // Divide by the sum
            for (int i = 0; i < birds.Count; i++)
            {
                birds[i].fitness = ((double)birds[i].score) / sum;
            }
        }


        // An algorithm for picking one bird from an array
        // based on fitness
        private Bird poolSelection(List<Bird> birds)
        {
            // Start at 0
            int index = 0;

            // Pick a random number between 0 and 1
            double r = random.NextDouble();

            // Keep subtracting probabilities until you get less than zero
            // Higher probabilities will be more likely to be fixed since they will
            // subtract a larger number towards zero
            while (r > 0)
            {
                r -= birds[index].fitness;
                // And move on to the next
                index += 1;
            }

            // Go back one
            index -= 1;

            // Make sure it's a copy!
            // (this includes mutation)
            return birds[index].copy();
        }


    }
}
