using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Species
    {
        //TODO: perks of species?

        //Attributes
        /// <summary>
        /// The name of the species
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the species
        /// </summary>
        public string Description { get; set; }

        //The avarage age this species reaches
        public int AverageAge { get; set; }
    }
}
