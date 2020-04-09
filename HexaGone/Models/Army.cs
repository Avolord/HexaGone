using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
