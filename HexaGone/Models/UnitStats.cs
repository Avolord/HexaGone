using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace HexaGone.Models
{
    static public class UnitStats
    {
        static public readonly List<Unit> AllUnits = new List<Unit>();

       static public string Test { get; set; }

        static public void PopulateUnits()
        {
            
            using (var reader = new StreamReader(@"C:\Users\Arvid\Source\Repos\Avolord\HexaGone\HexaGone\wwwroot\resources\UnitStats.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Unit newUnit = new Unit(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), values[2], (float)Convert.ToDouble(values[3]), (float)Convert.ToDouble(values[4]), Convert.ToInt32(values[5]), Convert.ToInt32(values[6]), values[7] == "1", (float)Convert.ToDouble(values[8]));
                    AllUnits.Add(newUnit);
                }
            }
            
        }

    }
}
