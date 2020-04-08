using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Army
    {
        public List<Unit> Units { get; }
        public void Destroy()
        {
            foreach (Unit n in Units)
            {
                n.MenCount = 0;
            }
        }

        public void AddUnit(Unit unit)
        {
            Units.Add(unit);
        }

    }
}
