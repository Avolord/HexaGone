using System.Collections.Generic;

namespace HexaGone.Models
{
    public class Army
    {
        public readonly List<Unit> Units = new List<Unit>();
        
        public void Destroy()
        {
            Units.Clear();
        }

        public void AddUnit(Unit unit)
        {
            Units.Add(unit);
        }

        public int GetTotalManCount()
        {
            int TotalManCount = 0;
            foreach (Unit n in Units)
            {
                TotalManCount += n.MenCount;
            }
            return TotalManCount;
        }
        public int GetTotalMaxManCount()
        {
            int TotalMaxManCount = 0;
            foreach (Unit n in Units)
            {
                TotalMaxManCount += n.MaxMenCount;
            }
            return TotalMaxManCount;
        }

    }
}
