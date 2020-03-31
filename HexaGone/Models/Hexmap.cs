using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexagonalGrid.Models
{
    public class Hexmap
    {
        public int width { get; set; }
        public int height { get; set; }

        public float hexWidth { get; set; }
        public float hexHeight { get; set; }

        public float hexSideLength { get; set; }

        public Hex[,] hexes { get; set; }
    }
}
