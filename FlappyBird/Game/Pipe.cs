using FlappyBird.Game;
using FlappyBird.Visual.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Visual.Game
{
    public class Pipe
    {
        private static Random r;
        public readonly int top;
        public readonly int bottom;
        public int x;
        public readonly int w;
        private readonly int speed;

        static Pipe()
        {
            r = Constants.r;
        }
        public Pipe()
        {
            // How big is the empty space
            var spacing = Constants.PipeConstants.spacing;
            // Where is th center of the empty space
            var centery = r.Next(spacing, GameManager.HEIGHT - spacing);

            // Top and bottom of pipe
            this.top = centery - spacing / 2;
            this.bottom = GameManager.HEIGHT - (centery + spacing / 2);
            // Starts at the edge
            this.x = GameManager.WIDTH;
            // Width of pipe
            this.w = Constants.PipeConstants.w;
            // How fast
            this.speed = Constants.PipeConstants.speed;
        }

        public bool hits(Bird bird)
        {
            if ((bird.y - bird.r) < this.top || (bird.y + bird.r) > (GameManager.HEIGHT - this.bottom))
            {
                if (bird.x > this.x && bird.x < this.x + this.w)
                {
                    return true;
                }
            }
            return false;
        }

        // Draw the pipe
        public void show(Graphics g)
        {
            Brush p = Brushes.Green;
            g.FillRectangle(p, this.x, 0, this.w, this.top);
            g.FillRectangle(p, this.x, GameManager.HEIGHT - this.bottom, this.w, this.bottom);
        }

        // Update the pipe
        public void update()
        {
            this.x -= this.speed;
        }

        // Has it moved offscreen?
        public bool offscreen()
        {
            if (this.x < -this.w)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
