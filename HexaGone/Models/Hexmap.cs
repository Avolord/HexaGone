using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Hexmap
    {
        public int width { get; set; }
        public int height { get; set; }

        public float hexWidth { get; set; }
        public float hexHeight { get; set; }

        public float hexSideLength { get; set; }

        public Hex[,] hexes { get; set; }
        public int[][] texture_index { get; set; }


        //===
        // Array-Tests
        public int[] v = new int[2];
        //===
    }
}
