using FlappyBird.Visual.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Visual.Game
{
    public class RandomUtils
    {
        private static bool _gaussian_previous = false;
        private static double y2 = 0;
        private static readonly Random r;

        static RandomUtils()
        {
            r = Constants.r;
        }


        public static double randomGaussian(double mean, double sd)
        {
            double y1, x1, x2, w;
            if (_gaussian_previous)
            {
                y1 = y2;
                _gaussian_previous = false;
            }
            else
            {
                do
                {
                    x1 = r.NextDouble() * 2 - 1;
                    x2 = r.NextDouble() * 2 - 1;
                    w = x1 * x1 + x2 * x2;
                } while (w >= 1);
                w = Math.Sqrt(-2 * Math.Log(w) / w);
                y1 = x1 * w;
                y2 = x2 * w;
                _gaussian_previous = true;
            }

            double m = mean;
            double s = sd;
            return y1 * s + m;
        }
    }
}
