using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    public class CityBuilding : Construction
    {
        /// <summary>
        /// A unique ID for every Building
        /// </summary>
        public int BuildingID { get; set; }
        /// <summary>
        /// The city where the Building was build in
        /// </summary>
        public City ParrentCity { get; set; }

        //Constructor
        /// <summary>
        /// Constructs a City from Parrent Field
        /// </summary>
        /// <param name="_ParrentCity">The Parrent City</param>
        public CityBuilding (City _ParrentCity )
        {
            BuildingID = CreateUniqueBuildingID();
            ParrentCity = _ParrentCity;
        }

        public int CreateUniqueBuildingID()
        {
            //TO ADD: Everything.
            return 0;
        }
    }
}
