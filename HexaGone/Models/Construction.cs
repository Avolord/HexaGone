using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Construction
    {
        /// <summary>
        /// Name of The Construction
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Does the Construction need a Field?
        /// </summary>
        public bool NeedsField { get; set; }
        /// <summary>
        /// How many Jobs does the Construction provide
        /// </summary>
        public int Jobs { get; set; }
        /// <summary>
        /// The Cost of the Construction
        /// </summary>
        public List<Resource> Cost { get; set; }
        /// <summary>
        /// The Influence of the Construction
        /// </summary>
        public int Influence { get; set; }
        /// <summary>
        /// The Defenseive Value of the Construction
        /// </summary>
        public int Defense { get; set; }
        /// <summary>
        /// Tha Attack Value of the Construction
        /// </summary>
        public int Attack { get; set; }
    }
}
