using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Road   :   Construction
    {
        //TODO: is a road a hexagon or the route across multiple hexagons?

        //Attributes
        /// <summary>
        /// The ID of the road
        /// </summary>
        public int RoadId { get; set; }
        /// <summary>
        /// Field on which the road is build
        /// </summary>
        public Field ParentField { get; set; }

        /// <summary>
        /// Second city the road is connecting
        /// </summary>
        public City CityOne { get; set; }

        /// <summary>
        /// Second city the road is connecting
        /// </summary> 
        public City CityTwo { get; set; }

        /// <summary>
        /// The total time it takes to get to ne next City
        /// </summary>
        public int TimeToCross { get; set; }
    }
}
