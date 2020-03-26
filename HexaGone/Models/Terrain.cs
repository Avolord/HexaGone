using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// Describes the type of terrain a field can have. For Example Water, Forest, Mountain etc.
    /// </summary>
    public class Terrain
    {
        //Attributes

        /// <summary>
        /// The ID of the terrain. 
        /// </summary>
        public int TerrainID { get; set; }
        /// <summary>
        /// The name of the terrain.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The factor of the speed at which entities can pass through the terrain.
        /// </summary>
        public double BaseMovementRate { get; set; }
        /// <summary>
        /// The factor of the visibility. The higher, the better an entity can see into and through the field.
        /// </summary>
        public double BaseVisibility { get; set; }
        /// <summary>
        /// The factor of the food/gold expenditures for resting armies.
        /// </summary>
        public double BaseExpenditures { get; set; }
        /// <summary>
        /// Says if the Terrain is a Land terrain, that normal units can walk on.
        /// </summary>
        public bool IsLand { get; set; }
        /// <summary>
        /// A List of the standard available Resources on a piece of Terrain.
        /// </summary>
        public List<int> TerrainResources { get; set; }

        //Constants 
        public const int terrainID_Water = 0;
        public const int terrainID_Plains = 1;
        public const int terrainID_Forest = 2;
        public const int terrainID_Mountains = 3;
        public const int terrainID_Desert = 4;

        //Constructor

        /// <summary>
        /// Constructs a Terrain from a _TerrainID. Currently supports IDs ranging from 0-4. 
        /// </summary>
        /// <param name="_TerrainID">
        /// For the constant IDs look at Terrain.terrainID_{Terrain}
        /// 0 = Water ;
        /// 1 = Plains ;
        /// 2 = Forest ;
        /// 3 = Mountains ;
        /// 4 = Desert ;
        /// defaults to Water
        /// </param>
        public Terrain(int _TerrainID)
        {
            TerrainID = _TerrainID;
            IsLand = true;
            TerrainResources = new List<int>();

            switch(TerrainID)
            {
                //Water
                case terrainID_Water:
                    Name = "Water";
                    BaseMovementRate = 1;
                    BaseVisibility = 2;
                    BaseExpenditures = 1;
                    IsLand = false;
                    TerrainResources.Add(Resource.resourceID_Food);
                    break;
                //Plains
                case terrainID_Plains:
                    Name = "Plains";
                    BaseMovementRate = 1;
                    BaseVisibility = 1.5;
                    BaseExpenditures = 1;
                    TerrainResources.Add(Resource.resourceID_Food);
                    break;
                //Forest
                case terrainID_Forest:
                    Name = "Forest";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceID_Food);
                    TerrainResources.Add(Resource.resourceID_Wood);
                    break;
                //Mountains
                case terrainID_Mountains:
                    Name = "Mountains";
                    BaseMovementRate = 0.5;
                    BaseVisibility = 0.75;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceID_Stone);
                    break;
                //Desert
                case terrainID_Desert:
                    Name = "Desert";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 1.5;
                    BaseExpenditures = 2;
                    break;

                //Default (in this case Water)
                default:
                    TerrainID = terrainID_Water;
                    Name = "Water";
                    BaseMovementRate = 1;
                    BaseVisibility = 2;
                    BaseExpenditures = 1;
                    IsLand = false;
                    TerrainResources.Add(Resource.resourceID_Food);
                    break;
            }
        }

        //Functions
    }
}
