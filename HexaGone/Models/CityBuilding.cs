using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    public class CityBuilding
    {
        /// <summary>
        /// A unique ID for every Building
        /// </summary>
        public int BuildingID { get; set; }
        /// <summary>
        /// The city where the Building was build in
        /// </summary>
        public City ParrentCity { get; set; }
    }
}
