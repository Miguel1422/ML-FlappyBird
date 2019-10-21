using FlappyBird.Game;
using FlappyBird.NN;
using FlappyBird.Visual.NN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Visual.Game
{
    public class Bird
    {
        public static double map(double val, double min, double max, double rMin, double rMax)
        {

            if (val < min || val > max)
            {
                // throw new Exception("Fuera de rango");
            }

            double range = max - min;
            double resRange = rMax - rMin;

            double res = resRange * (val - min) / range;

            return res + rMin;
        }

        public readonly int x;
        public double y;
        public readonly int r;
        public readonly double gravity;
        public readonly int lift;
        public double velocity;
        public readonly NeuralNetwork brain;
        public static readonly Random random;
        public int score;
        public double fitness;

        static Bird()
        {
            random = Constants.r;
        }



        private static Matrix.MapFunction mutate = (x) =>
        {
            if (random.NextDouble() < 0.1)
            {
                double offset = RandomUtils.randomGaussian(0, 1) * 0.5;
                return x + offset;
            }
            return x;
        };

        public Bird(NeuralNetwork brain)
        {
            // position and size of bird
            this.x = 64;
            this.y = GameManager.HEIGHT / 2;
            this.r = 12;

            // Gravity, lift and velocity
            this.gravity = 0.8;
            this.lift = -12;
            this.velocity = 0;

            // Is this a copy of another Bird or a new one?
            // The Neural Network is the bird's "brain"

            this.brain = new NeuralNetwork(brain);
            this.brain.mutate(mutate);


            // Score is how many frames it's been alive
            this.score = 0;
            // Fitness is normalized version of score
            this.fitness = 0;
        }
        public Bird() : this(new NeuralNetwork(5, 8, 2))
        {
        }
        public Bird copy()
        {
            return new Bird(this.brain);
        }

        public void show(Graphics g)
        {
            Brush brush = Brushes.Black;
            g.FillEllipse(brush, this.x, (float)this.y, this.r * 2, this.r * 2);
        }

        public void Think(List<Pipe> pipes)
        {
            // First find the closest pipe
            Pipe closest = null;
            double record = Double.PositiveInfinity;
            for (int i = 0; i < pipes.Count; i++)
            {
                int diff = (pipes[i].x + pipes[i].w) - this.x;
                if (diff > 0 && diff < record)
                {
                    record = diff;
                    closest = pipes[i];
                }
            }

            if (closest != null)
            {
                // Now create the inputs to the neural network
                double[] inputs = new double[5];
                // x position of closest pipe
                inputs[0] = map(closest.x, this.x, GameManager.WIDTH, 0, 1);
                // top of closest pipe opening
                inputs[1] = map(closest.top, 0, GameManager.HEIGHT, 0, 1);
                // bottom of closest pipe opening
                inputs[2] = map(closest.bottom, 0, GameManager.HEIGHT, 0, 1);
                // bird's y position
                inputs[3] = map(this.y, 0, GameManager.HEIGHT, 0, 1);
                // bird's y velocity
                inputs[4] = map(this.velocity, -5, 5, 0, 1);

                // Get the outputs from the network
                var action = this.brain.predict(inputs);
                // Decide to jump or not!
                if (action[1] > action[0])
                {
                    this.up();
                }
            }
        }

        // Jump up
        public void up()
        {
            this.velocity += this.lift;
        }

        public bool bottomTop()
        {
            // Bird dies when hits bottom?
            return (this.y > GameManager.HEIGHT || this.y < 0);
        }

        // Update bird's position based on velocity, gravity, etc.
        public void update()
        {
            this.velocity += this.gravity;
            // this.velocity *= 0.9;
            this.y += this.velocity;

            // Every frame it is alive increases the score
            this.score++;
        }


    }
}
