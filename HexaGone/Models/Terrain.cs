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
        public List<int> TerrainResources { get; }

        //Constants
        public const int Plains = 0;
        public const int Trees = 1;
        public const int Forest = 2;
        public const int Hills = 3;
        public const int HillTrees = 4;
        public const int Mountain = 5;

        public const int ShallowWater = 6;
        public const int Ocean = 7;

        public const int SwampTrees = 12;
        public const int SwampPond = 13;
        public const int SwampPlains = 14;
        public const int SwampWater = 15;

        public const int SnowPlains = 16;
        public const int SnowTrees = 17;
        public const int SnowForest = 18;
        public const int SnowHills = 19;
        public const int SnowHillTrees = 20;
        public const int IceWater = 21;

        public const int DesertPlains = 24;
        public const int DesertHills = 25;
        public const int DesertDunes = 26;
        public const int DesertMountain = 27;
        public const int DesertOasis = 28;

        public const int Jungle = 32;

        /// <summary>
        /// The amount of defined terrains. If you add a terrain at the top and in the constructor in the switch-case statement, increase this number by 1.
        /// </summary>
        public const int amountTerrains = 24;
        

        //Constructor

        /// <summary>
        /// Constructs a Terrain from a _TerrainID. Currently supports IDs ranging from 0-4. 
        /// </summary>
        /// <param name="terrainID">
        /// For the constant IDs look at Terrain.terrainID_{Terrain}
        /// 0 = Water ;
        /// 1 = Plains ;
        /// 2 = Forest ;
        /// 3 = Mountains ;
        /// 4 = Desert ;
        /// defaults to Water
        /// </param>
        public Terrain(int terrainID)
        {
            TerrainID = terrainID;
            IsLand = true;
            TerrainResources = new List<int>();

            switch(TerrainID)
            {
                //Ocean
                case Ocean:
                    Name = "Water";
                    BaseMovementRate = 1;
                    BaseVisibility = 2;
                    BaseExpenditures = 1;
                    IsLand = false;
                    TerrainResources.Add(Resource.resourceIdFood);
                    break;
                //Shallow Water
                case ShallowWater:
                    Name = "Shallow Water";
                    BaseMovementRate = 1;
                    BaseVisibility = 2;
                    BaseExpenditures = 1;
                    IsLand = false;
                    TerrainResources.Add(Resource.resourceIdFood);
                    break;
                //Plains
                case Plains:
                    Name = "Plains";
                    BaseMovementRate = 1;
                    BaseVisibility = 1.5;
                    BaseExpenditures = 1;
                    TerrainResources.Add(Resource.resourceIdFood);
                    break;
                //Forest
                case Forest:
                    Name = "Forest";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //Mountains
                case Mountain:
                    Name = "Mountains";
                    BaseMovementRate = 0.5;
                    BaseVisibility = 0.75;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdStone);
                    break;
                //Desert
                case DesertPlains:
                    Name = "Desert";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 1.5;
                    BaseExpenditures = 2;
                    break;


                //These Terrains have the same properties as forests, they were just copy pasted for functionality

                //Jungle
                case Jungle:
                    Name = "Jungle";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //SwampTrees
                case SwampTrees:
                    Name = "Swamp Trees";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //SnowTrees
                case SnowTrees:
                    Name = "Snow Trees";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //Hills
                case Hills:
                    Name = "Hills";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //Hill Trees
                case HillTrees:
                    Name = "Hill Trees";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //DesertMountain
                case DesertMountain:
                    Name = "Desert Mountain";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //DesertHills
                case DesertHills:
                    Name = "Desert Hills";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //SwampPlains
                case SwampPlains:
                    Name = "Swamp Plains";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //SwampPond
                case SwampPond:
                    Name = "Swamp Pond";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;
                //SnowHillTrees
                case SnowHillTrees:
                    Name = "Snow Hill Trees";
                    BaseMovementRate = 0.8;
                    BaseVisibility = 0.25;
                    BaseExpenditures = 1.2;
                    TerrainResources.Add(Resource.resourceIdFood);
                    TerrainResources.Add(Resource.resourceIdWood);
                    break;



                //Default (in this case Water)
                default:
                    TerrainID = Ocean;
                    Name = "Water";
                    BaseMovementRate = 1;
                    BaseVisibility = 2;
                    BaseExpenditures = 1;
                    IsLand = false;
                    TerrainResources.Add(Resource.resourceIdFood);
                    break;
            }
        }

        //Functions
    }
}
