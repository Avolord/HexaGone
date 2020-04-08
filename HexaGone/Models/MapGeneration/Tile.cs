using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.MapGeneration
{
    public class Tile
    {
        //Attributes
        public int Terrain { get; set; }
        public int Height { get; set; }
        public int Humidity { get; set; }

        //Constructor
        public Tile()
        { 
            Terrain = 0;
            Height = 0;
            Humidity = 0;
        }

        //Functions
    }
}
