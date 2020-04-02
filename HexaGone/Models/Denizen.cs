using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Denizen
    {
        //TODO: stats for denizen?

        //Attributes
        /// <summary>
        /// The city where the denizen lives
        /// </summary>
        public City Hometown { get; set; }  

        /// <summary>
        /// The species of the denizen
        /// </summary>
        public Species DenizenSpecies { get; set; }

        /// <summary>
        /// The age of the denizen
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// The job of the denizen
        /// </summary>
        public Job DenizenJob { get; set; }
    }
}
