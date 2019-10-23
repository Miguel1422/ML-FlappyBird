using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird.Visual.Game
{
    public class Constants
    {
        
        public static readonly Random r = new Random(123);

        
        public class BirdConstants
        {
            // Radius
            public const int r = 12;

            // Gravity, lift
            public const double gravity = 0.8;
            public const int lift = -12;
        }
        public class PipeConstants
        {
            public const int spacing = 100;
            public const int speed = 6;
            public const int w = 80;

            
            public const int newPipeEveryFrames = 75;
        }

    }
}
