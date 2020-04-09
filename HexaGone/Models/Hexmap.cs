using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace HexaGone.Models
{
    public class Hexmap
    {
        public Hexmap()
        {
            // Fill in mapData with information from a .csv file.
            using (var reader = new StreamReader(@"wwwroot/map/HexaGone_map.csv"))
            {
                mapData = new List<List<int>>();
                while (!reader.EndOfStream)
                {
                    mapData.Add(new List<int>());
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    foreach(string s in values)
                    {
                        mapData.Last().Add(int.Parse(s));
                    }
                }
            }
        }

        /// <summary>
        /// Sidelength of a hexagon.
        /// </summary>
        public float hexSideLength { get; set; }

        /// <summary>
        /// Two-Dimensional List which contains the information about the map.
        /// </summary>
        public List<List<int>> mapData;
    }
}
