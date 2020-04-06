using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Map
    {
        //Attributes
        public List<List<Field>> Fields { get; }
        public int Width { get; }
        public int Height { get; }

        //Constants

        //mapModes - A constant mapMode the host can choose from, that generates the map.
        /// <summary>
        /// Creates a completely random map. Every Field has a randomly chosen terrain.
        /// </summary>
        public const int mapModeCompletelyRandom = 0;
        public const int mapModeTinyIslands = 1;
        

        //Constructor
        //public Map(int mapMode, int sizeMode)
        //{

        //}
        public Map(int mapMode, int width, int height)
        {
            //Set Width and Height
            Width = width;
            Height = height;

            //Generate the Map
            MapgeneratorSwitcher(mapMode);

        }

        //Functions
        /// <summary>
        /// Selects a mapgeneration function from the mapMode.
        /// </summary>
        /// <param name="mapMode">A constant mapMode the host can choose from, that generates the map.</param>
        private void MapgeneratorSwitcher(int mapMode)
        {
            //The while-loop has the purpose to go into the switch-case statement a second time, if the mapMode goes into the default-statement.
            while(Fields.Count == 0)
            {
                switch(mapMode)
                {
                    //Creates a completely random map. Every Field has a randomly chosen terrain.
                    case mapModeCompletelyRandom:
                        CompletelyRandomMapgenerator();
                        break;

                    case mapModeTinyIslands:
                        TinyIslandsMapgenerator();
                        break;

                    //sets the mapMode and goes again into the switch-case statement, to use a valid mapMode this time.
                    default:
                        mapMode = mapModeCompletelyRandom;
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a completely random map. Every Field has a randomly chosen terrain.
        /// </summary>
        private void CompletelyRandomMapgenerator()
        {
            Random random = new Random();
            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for(int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for(int row = 0; row < Height; row++)
                {
                    Field field = new Field(column, row, random.Next(0, Terrain.amountTerrains));
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }
        }
        private void TinyIslandsMapgenerator()
        {
            Random random = new Random();
            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for (int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    Field field;
                    if((column + row)%3 == 0)
                    {
                        field = new Field(column, row, random.Next(1, Terrain.amountTerrains));
                    }
                    else
                    {
                        field = new Field(column, row, Terrain.terrainIdWater);
                    }
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }
        }
    }
}
