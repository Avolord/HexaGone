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
        public bool IsPointy { get; }
        public int HexSideLength { get; set; }

        //Constants

        //mapModes - A constant mapMode the host can choose from, that generates the map.
        /// <summary>
        /// Creates a completely random map. Every Field has a randomly chosen terrain.
        /// </summary>
        public const int mapModeCompletelyRandom = 0;
        public const int mapModeTinyIslands = 1;
        public const int mapModeOneIsland = 2;
        

        //Constructor
        //public Map(int mapMode, int sizeMode)
        //{

        //}
        public Map(int mapMode, int width, int height)
        {
            //Set Width and Height
            Width = width;
            Height = height;
            IsPointy = false;
            HexSideLength = 30;
            Fields = new List<List<Field>>();

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
                    case mapModeOneIsland:
                        OneIslandMapgenerator();
                        break;

                    //sets the mapMode and goes again into the switch-case statement, to use a valid mapMode this time.
                    default:
                        mapMode = mapModeCompletelyRandom;
                        break;
                }
            }
        }

        private Field GetField(int column, int row)
        {
            return Fields[column][row];
        }
        private Field GetField(Coordinates coordinates)
        {
            return Fields[coordinates.Column][coordinates.Row];
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
        private void OneIslandMapgenerator()
        {
            Random random = new Random();
            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for (int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    Field field = new Field(column, row, Terrain.terrainIdWater);
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }

            //Select a random new Field
            int randX = Convert.ToInt32(Width / 2);
            int randY = Convert.ToInt32(Height / 2);
            //Set it to Plains
            Fields[randX][randY] = new Field(randX, randY, Terrain.terrainIdDesert);
            System.Diagnostics.Debug.WriteLine("1: " + randX + " ; " + randY);
            //Create a List with all neighbours
            List<Coordinates> neighbours = new List<Coordinates>();
            AddNeighbours(ref neighbours, Fields[randX][randY]);
            //Set a number of max Land Fields. In this case it's 40%
            int maxLandFields = 100;
            int currentLandFields = 1;

            //Select random neighbour fields and make them land
            while(currentLandFields <= maxLandFields)
            {
                int randomIndex = random.Next(0, neighbours.Count);
                Coordinates coordinates = neighbours[randomIndex];

                if(GetField(coordinates).FieldTerrain.TerrainID == Terrain.terrainIdWater)
                {
                    Field field = Fields[coordinates.Column][coordinates.Row];
                    Fields[coordinates.Column][coordinates.Row] = new Field(coordinates.Column, coordinates.Row, Terrain.terrainIdPlains);
                    AddNeighbours(ref neighbours, field);
                    currentLandFields += 1;

                    System.Diagnostics.Debug.WriteLine(currentLandFields + ": " + coordinates.Column + " ; " + coordinates.Row);
                }
                else
                {
                    neighbours.RemoveAt(randomIndex);
                }
            }
            return;
        }

        private void AddNeighbours(ref List<Coordinates> neighbours, Field field)
        {

            for (int i = 0; i < 6; i++)
            {
                int column = field.Coordinates.Column;
                int row = field.Coordinates.Row;
                Coordinates coordinates = new Coordinates();
                switch(i)
                {
                    //On top
                    case 0:
                        coordinates.Column = column;
                        coordinates.Row = row - 1;
                        break;
                    //top right even/bottom right odd
                    case 1:
                        if (field.Coordinates.Column % 2 == 0)
                        {
                            coordinates.Column = column + 1;
                            coordinates.Row = row -1;
                            break;
                        }
                        else
                        {
                            coordinates.Column = column + 1;
                            coordinates.Row = row +1;
                            break;
                        }
                    //bottom right even/top right odd
                    case 2:
                        coordinates.Column = column + 1;
                        coordinates.Row = row;
                        break;
                    //bottom
                    case 3:
                        coordinates.Column = column;
                        coordinates.Row = row + 1;
                        break;
                    //bottom left even/top left odd
                    case 4:
                        coordinates.Column = column - 1;
                        coordinates.Row = row;
                        break;
                    //top left even/bottom left odd
                    case 5:
                        if (field.Coordinates.Column % 2 == 0)
                        {
                            coordinates.Column = column - 1;
                            coordinates.Row = row - 1;
                            break;
                        }
                        else
                        {
                            coordinates.Column = column - 1;
                            coordinates.Row = row + 1;
                            break;
                        }
                }

                if(IsCoordinateInRange(coordinates) && GetField(coordinates).FieldTerrain.TerrainID == Terrain.terrainIdWater && !IsBorderCoordinate(coordinates))
                {
                    neighbours.Add(coordinates);
                }
            }
        }
        private bool IsBorderCoordinate(Coordinates coordinates)
        {
            if(coordinates.Column == 0 || coordinates.Column == Width-1 ||coordinates.Row == 0 || coordinates.Row == Height -1)
            {
                return true;
            }
            return false;
        }
        private bool IsCoordinateInRange(Coordinates coordinates)
        {
            if(coordinates.Column >= 0 && coordinates.Column < Width && coordinates.Row >= 0 && coordinates.Row < Height)
            {
                return true;
            }
            return false;
        }
    }
}
