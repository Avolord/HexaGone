using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Job
    {
        //Attributes
        /// <summary>
        ///  The ID of the Job
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        ///  The Name of the Job
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the Job
        /// </summary>
        public string Description { get; set; }
    }
}