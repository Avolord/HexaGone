using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.MapGeneration
{
    public class Tile
    {
        //Attributes
        public int BiomeID { get; set; }
        public int Height { get; set; }
        public int Humidity { get; set; }

        //Constructor
        public Tile()
        { 
            BiomeID = -1;
            Height = 0;
            Humidity = 0;
        }

        //Functions
    }
}
