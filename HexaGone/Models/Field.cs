using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// One hexagon Field in the world.
    /// </summary>
    public class Field
    {
        //Attributes
        /// <summary>
        /// A unique ID for the Field.
        /// </summary>
        public int FieldID { get; set; }
        /// <summary>
        /// The coordinates on the Map. Are given at initialisation.
        /// </summary>
        public int[] Coordinates { get; set; }
        /// <summary>
        /// The Terrain at this Field. Is given at initialisation.
        /// </summary>
        public Terrain FieldTerrain { get; set; }
        /// <summary>
        /// The height of this Field. Can be given at initialisation.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The Empire this Field belongs to. Can be null if it doesnt belong to any.
        /// </summary>
        public Empire BelongsToEmpire { get; set; }
        /// <summary>
        /// The City this Field belongs to. Can be null if it doesnt belong to any.
        /// </summary>
        public City BelongsToCity { get; set; }
        /// <summary>
        /// The Construction on this Field. It cant have a FieldBuilding if it is a city and vice versa.
        /// </summary>
        public Construction Construction { get; set; }
        /// <summary>
        /// A list of the available resources on this Field.
        /// </summary>
        public List<int> FieldResources { get; set; }
        //TO ADD: Streets
        /// <summary>
        /// The factor of the speed at which entities can pass through the terrain.
        /// </summary>
        public double MovementRate { get; set; }
        //TO ADD: List of MovementRateModifiers
        /// <summary>
        /// The factor of the visibility. The higher, the better an entity can see into and through the field.
        /// </summary>
        public double Visibility { get; set; }
        //TO ADD: List of VisibilityModifiers
        /// <summary>
        /// The factor of the food/gold expenditures for resting armies.
        /// </summary>
        public double Expenditures { get; set; }
        //TO ADD: List of ExpendituresModifiers


        //Constructor
        /// <summary>
        /// Constructs a Field from its coordinates, its Terrain and optionally from a height.
        /// </summary>
        /// <param name="_MapX">The X-Coordinate of the Field</param>
        /// <param name="_MapY">The Y-Coordinate of the Field</param>
        /// <param name="_FieldTerrainID">The TerrainID. For the constant IDs look at Terrain.terrainID_{Terrain}</param>
        /// <param name="_Height">The Height of this Field. Optional. Defaults to 0.</param>
        public Field(int _MapX, int _MapY, int _FieldTerrainID, int _Height = 0)
        {
            FieldID = CreateUniqueFieldID();
            Coordinates = new int[2] { _MapX, _MapY };
            FieldTerrain = new Terrain(_FieldTerrainID);
            Height = _Height;
            BelongsToEmpire = null;
            BelongsToCity = null;
            FieldResources = new List<int>();
            FillFieldResourcesFromTerrainResources();

            //TO DO: Calculate these three Attributes from the Lists instead of getting them directly from the Terrain.
            MovementRate = FieldTerrain.BaseMovementRate;
            Visibility = FieldTerrain.BaseVisibility;
            Expenditures = FieldTerrain.BaseExpenditures;
        }


        //Functions

        /// <summary>
        /// WIP. Creates a unique ID for the Field.
        /// </summary>
        /// <returns></returns>
        public int CreateUniqueFieldID()
        {
            //TO ADD: Everything.
            return 0;
        }
        /// <summary>
        /// Fills the List of Resources on the Field from the List of Resources on the Terrain.
        /// </summary>
        public void FillFieldResourcesFromTerrainResources()
        {
            foreach(int resource in FieldTerrain.TerrainResources)
            {
                FieldResources.Add(resource);
            }
        }

    }
}
