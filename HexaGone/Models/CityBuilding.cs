using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// A building in a city.
    /// </summary>
    public class CityBuilding : Construction
    {
        /// <summary>
        /// A unique ID for the Building
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// An ID for the type of Building.
        /// </summary>
        public int BuildingID { get; set; }
        /// <summary>
        /// The city where the Building is build in
        /// </summary>
        public City ParentCity { get; set; }
        /// <summary>
        /// The upgrade level of the building.
        /// </summary>
        public int UpgradeLevel { get; set; }
        /// <summary>
        /// Says if this building needs a building space in the city or not.
        /// </summary>
        public bool NeedsBuildingSpace { get; set; }

        //Constants

        //Add constant building IDs for the buildings.
        //public const int

        //Constructor
        /// <summary>
        /// Constructs a CityBuilding from Parent City and BuildingID.
        /// </summary>
        /// <param name="_ParentCity">The Parrent City</param>
        public CityBuilding (City _ParentCity, int _BuildingID)
        {
            ID = CreateUniqueCityBuildingID();
            BuildingID = _BuildingID;
            ParentCity = _ParentCity;
            NeedsField = false;

            switch(BuildingID)
            {
                //Add cases for BuildingIDs here for specific properties and names.

                default:
                    //do nothing
                    break;
            }
        }

        public int CreateUniqueCityBuildingID()
        {
            //TO ADD: Everything.
            return 0;
        }
    }
}
