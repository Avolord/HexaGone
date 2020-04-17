using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.MapGeneration
{
    public class Biome
    {
        //Attributes
        public int ID { get; set; }
        public int Weight { get; set; }
        public List<Coordinates> Neighbours { get; }

        //Constants
        //public static readonly Biome Ocean = new Biome();
        public const int Plains = 0;
        public const int Forest = 1;
        public const int Desert = 2;
        public const int Lake = 3;
        public const int Jungle = 4;
        public const int Swamp = 5;
        public const int Tundra = 6;
        public const int Ocean = 7;

        //Constructor
        public Biome(int id)
        {
            ID = id;
            Weight = -1;
            Neighbours = new List<Coordinates>();

            while (Weight == -1)
            {
                switch (ID)
                {
                    case Plains:
                        Weight = 10;
                        break;
                    case Forest:
                        Weight = 10;
                        break;
                    case Desert:
                        Weight = 5;
                        break;
                    case Lake:
                        Weight = 2;
                        break;
                    case Jungle:
                        Weight = 1;
                        break;
                    case Swamp:
                        Weight = 1;
                        break;
                    case Tundra:
                        Weight = 5;
                        break;
                    case Ocean:
                        Weight = 0;
                        break;
                    default:
                        ID = Ocean;
                        break;

                }
            }
        }


    }
}
