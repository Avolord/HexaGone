using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace HexaGone.Models
{
    static public class UnitStats
    {
        static public readonly List<UnitSettings> AllUnits = new List<UnitSettings>();

       static public string Test { get; set; }

        static public void PopulateUnits()
        {
            
            using (var reader = new StreamReader(@"C:\Users\felix\source\repos\Avolord\HexaGone\HexaGone\wwwroot\resources\UnitStats.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    //Unit newUnit = new Unit(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), values[2], (float)Convert.ToDouble(values[3]), (float)Convert.ToDouble(values[4]), Convert.ToInt32(values[5]), Convert.ToInt32(values[6]), values[7] == "1", (float)Convert.ToDouble(values[8]));
                    UnitSettings newUnitSettings = new UnitSettings(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), values[2], (float)Convert.ToDouble(values[3]), (float)Convert.ToDouble(values[4]), Convert.ToInt32(values[5]), Convert.ToInt32(values[6]), values[7] == "1", (float)Convert.ToDouble(values[8]));

                    AllUnits.Add(newUnitSettings);
                }
            }
            
        }

    }
    public class UnitSettings
    {

        public UnitSettings(int unitID, int unitClass, string name, float attackStat, float defenseStat, int maxMenCount, int menCount, bool isRangedUnit, float movementPoints)
        {
            UnitID = unitID;
            UnitClass = unitClass;
            Name = name;
            AttackStat = attackStat;
            DefenseStat = defenseStat;
            MaxMenCount = maxMenCount;
            MenCount = menCount;
            IsRangedUnit = isRangedUnit;
            MovementPoints = movementPoints;
        }
        public int UnitID { get; set; }
        public int UnitClass { get; set; }
        public string Name { get; set; }
        public float AttackStat { get; set; }
        public float DefenseStat { get; set; }
        public int MaxMenCount { get; set; }
        public int MenCount { get; set; }
        public bool IsRangedUnit { get; set; }
        public float MovementPoints { get; set; }
    }
}
