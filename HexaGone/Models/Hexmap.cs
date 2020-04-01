using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Hexmap
    {
        public bool isPointy;
        public int width { get; set; }
        public int height { get; set; }

        public float hexWidth { get; set; }
        public float hexHeight { get; set; }
        public float hexSideLength { get; set; }
        
        public int[][] texture_index { get; set; }


        public void calculate()
        {
            if (isPointy)
            {
                hexWidth = (float)Math.Sqrt(3) * hexSideLength;
                hexHeight = 2 * hexSideLength;
            }
            else
            {

            }
        }
    }
}
