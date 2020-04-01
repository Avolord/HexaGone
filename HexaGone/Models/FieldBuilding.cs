using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// A building on a hexagon field.
    /// </summary>
    public class FieldBuilding : Construction
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
        /// The Field this FieldBuilding is build on.
        /// </summary>
        public Field ParentField { get; set; }
        /// <summary>
        /// The city this Building belongs to.
        /// </summary>
        public City ParentCity { get; set; }
        /// <summary>
        /// The upgrade level of the building.
        /// </summary>
        public int UpgradeLevel { get; set; }
        /// <summary>
        /// A List of terrains this building can be build on.
        /// </summary>
        public List<Terrain> LegalTerrains { get; set; }

        //Constants

        //Add constant building IDs for the buildings.
        //public const int

        //Constructor
        /// <summary>
        /// Constructs a FieldBuilding from Parent City and BuildingID.
        /// </summary>
        /// <param name="_ParentCity">The Parrent City</param>
        public FieldBuilding(City _ParentCity, Field _ParentField, int _BuildingID)
        {
            ID = CreateUniqueFieldBuildingID();
            BuildingID = _BuildingID;
            ParentField = _ParentField;
            ParentCity = _ParentCity;
            NeedsField = true;
            LegalTerrains = new List<Terrain>();

            switch (BuildingID)
            {
                //Add cases for BuildingIDs here for specific properties and names.

                default:
                    //do nothing
                    break;
            }
        }

        public int CreateUniqueFieldBuildingID()
        {
            //TO ADD: Everything.
            return 0;
        }
    }
}

