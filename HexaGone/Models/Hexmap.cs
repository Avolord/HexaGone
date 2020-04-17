using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Hexmap
    {
        public Hexmap()
        {
            using (var reader = new StreamReader(@"wwwroot/resources/HexaGone_map.csv"))
            {
                mapData = new List<List<int>>();
                while (!reader.EndOfStream)
                {
                    mapData.Add(new List<int>());
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    foreach (string s in values)
                    {
                        mapData.Last().Add(int.Parse(s));
                    }
                }
            }
        }

        public bool isPointy;
        public int width { get; set; }
        public int height { get; set; }

        public float hexWidth { get; set; }
        public float hexHeight { get; set; }
        public float hexSideLength { get; set; }
        
        public int[][] texture_index { get; set; }

        public List<List<int>> mapData { get; set; }


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
