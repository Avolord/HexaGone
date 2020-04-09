using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexaGone.Models.MapGeneration;

namespace HexaGone.Models
{
    public class Map
    {
        //Attributes
        public List<List<Field>> Fields { get; }
        public int Width { get; }
        public int Height { get; }
        public int BiomeSize { get; }
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
        public const int mapModeBiomes = 3;

        public const int Small = 0;
        public const int Medium = 1;
        public const int Big = 2;

        //Constructor
        public Map(int mapMode, int sizeMode, int biomeSize)
        {
            switch (sizeMode)
            {
                //Creates a completely random map. Every Field has a randomly chosen terrain.
                case Small:
                    Width = 100;
                    Height = 100;
                    break;
                case Medium:
                    Width = 200;
                    Height = 200;
                    break;
                case Big:
                    Width = 300;
                    Height = 300;
                    break;
                //sets the mapSize to  200 x 200
                default:
                    Width = 200;
                    Height = 200;
                    break;
            }
            BiomeSize = biomeSize;
            IsPointy = false;
            HexSideLength = 30;
            Fields = new List<List<Field>>();

            //Generate the Map
            MapgeneratorSwitcher(mapMode);
        }
        public Map(int mapMode, int width, int height, int biomeSize)
        {
            //Set Width and Height
            Width = width;
            Height = height;
            BiomeSize = biomeSize;
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
                    case mapModeBiomes:
                        BiomesMapgenerator();
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
                        field = new Field(column, row, Terrain.Ocean);
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
                    Field field = new Field(column, row, Terrain.Ocean);
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }

            //Select a random new Field
            int randX = Convert.ToInt32(Width / 2);
            int randY = Convert.ToInt32(Height / 2);
            //Set it to Plains
            Fields[randX][randY] = new Field(randX, randY, Terrain.DesertPlains);
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

                if(GetField(coordinates).FieldTerrain.TerrainID == Terrain.Ocean)
                {
                    Field field = Fields[coordinates.Column][coordinates.Row];
                    Fields[coordinates.Column][coordinates.Row] = new Field(coordinates.Column, coordinates.Row, Terrain.Plains);
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

        private void BiomesMapgenerator()
        {
            //A 2-Dimensional List containing all the tiles with its biomes.
            Random random = new Random();
            List<List<Tile>> tiles = new List<List<Tile>>();
            //Create a List of all the biomes, that are created
            List<Biome> biomes = new List<Biome>();
            //Create a List with the biome probabilities. A biome that is more probable to be select, is more often in this list.
            List<int> biomeProbability = new List<int>();



            //Set a number of max Land Fields. In this case it's 40%

            int currentLandFields = GenerateBiomes(ref tiles, ref biomes, ref biomeProbability); ;
            int maxLandFields = Convert.ToInt32(Width*Height *0.4);

            //Select random neighbour fields and make them land
            while (currentLandFields <= maxLandFields)
            {
                //First select a random biome, that gets a new tile
                int randomBiomeIndex = biomeProbability[random.Next(0, biomeProbability.Count)];
                Biome randomBiome = biomes[randomBiomeIndex];

                //Check if the random biome has any neighbours
                if (randomBiome.Neighbours.Count > 0)
                {
                    //Select a random neighbour from its Neighbours List.
                    int randomIndex = random.Next(0, randomBiome.Neighbours.Count);
                    Coordinates coordinates = randomBiome.Neighbours[randomIndex];

                    //Check if the neighbour tile is Ocean, in this case it can be set
                    if (tiles[coordinates.Column][coordinates.Row].BiomeID == Biome.Ocean)
                    {
                        Tile tile = tiles[coordinates.Column][coordinates.Row];
                        tiles[coordinates.Column][coordinates.Row].BiomeID = randomBiome.ID;
                        AddNeighbourTiles(randomBiome, ref tiles, coordinates);
                        currentLandFields += 1;

                        //System.Diagnostics.Debug.WriteLine(currentLandFields + ": " + coordinates.Column + " ; " + coordinates.Row);
                    }

                    //Remove the neighbour from the Neighbours List
                    randomBiome.Neighbours.RemoveAt(randomIndex);
                }
                //Check if the random biome has any neighbours. If not, remove it from the probability List
                if (randomBiome.Neighbours.Count == 0)
                {
                    //biomes.RemoveAt(randomBiomeIndex);
                    biomeProbability.RemoveAll(item => item == randomBiomeIndex);
                }
            }
            //Translate Tilemap biomes into Terrains.

            //Create List of Fields with the Terrains from biomes.
            for (int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    int terrainID = GetTerrainIdFromBiomeId(tiles[column][row].BiomeID);
                    Field field = new Field(column, row, terrainID);
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }
            return;
        }

        /// <summary>
        /// This methode creates Biomes based on Mapsize and biome size selected. Returns amount of current lanfields
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="biomes"></param>
        /// <param name="biomeProbability"></param>
        private int GenerateBiomes(ref List<List<Tile>> tiles, ref List<Biome> biomes, ref List<int>biomeProbability)
        {
            Random random = new Random();
            int landFields = 0;
            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for (int column = 0; column < Width; column++)
            {
                List<Tile> columnList = new List<Tile>();
                //The second loop iterates over the Height, to create as many Tiles as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    //At first every tile-biome will be ocean
                    Tile tile = new Tile()
                    {
                        BiomeID = Biome.Ocean
                    };
                    columnList.Add(tile);
                }
                //Add the columnList to tiles.
                tiles.Add(columnList);
            }

            //This loop is for the creation of the biomes.
            for (int i = 0; i < 9; i++)
            {
                //Select the starting tile of the biome
                int startX = Convert.ToInt32((Width / 4) + (Width / 12) + ((i % 3) * Width / 6));
                int startY = Convert.ToInt32((Height / 4) + (Height / 12) + ((Convert.ToInt32(i / 3)) * Height / 6));
                Coordinates coordinates = new Coordinates(startX, startY);
                Biome biome;
                //Set the biome-type of the biome
                switch (i)
                {
                    case 0:
                        biome = new Biome(Biome.Desert);
                        break;
                    case 1:
                        biome = new Biome(Biome.Forest);
                        break;
                    case 2:
                        biome = new Biome(Biome.Plains);
                        break;
                    case 3:
                        biome = new Biome(Biome.Jungle);
                        break;
                    case 4:
                        biome = new Biome(Biome.Lake);
                        break;
                    case 5:
                        biome = new Biome(Biome.Swamp);
                        break;
                    case 6:
                        biome = new Biome(Biome.Tundra);
                        break;
                    case 7:
                        biome = new Biome(Biome.Plains);
                        break;
                    //case 8
                    default:
                        biome = new Biome(Biome.Forest);
                        break;
                }

                //Add the biome to the biomes List.
                biomes.Add(biome);
                landFields++;
                //Give the starting-Tile the biome ID.
                tiles[startX][startY].BiomeID = biome.ID;

                //System.Diagnostics.Debug.WriteLine(i + ": " + startX + " ; " + startY);

                //Add Neighbour tiles of the starting field to the List of neighbours 
                AddNeighbourTiles(biome, ref tiles, coordinates);

                //Add the biome-index as much to the probability list, as their weight is.
                for (int w = 0; w < biome.Weight; w++)
                {
                    biomeProbability.Add(i);
                }

            }
            return landFields;
        }

        private int GetTerrainIdFromBiomeId(int biomeId)
        {
            switch(biomeId)
            {
                case Biome.Plains:
                    return Terrain.Plains;
                case Biome.Forest:
                    return Terrain.Forest;
                case Biome.Desert:
                    return Terrain.DesertPlains;
                case Biome.Lake:
                    return Terrain.ShallowWater;
                case Biome.Jungle:
                    return Terrain.Jungle;
                case Biome.Swamp:
                    return Terrain.SwampTrees;
                case Biome.Tundra:
                    return Terrain.SnowTrees;
                case Biome.Ocean:
                    return Terrain.Ocean;
                default:
                    return Terrain.Mountain;
            }
        }
        private void AddNeighbours(ref List<Coordinates> neighbours, Field field)
        {

            for (int i = 0; i < 6; i++)
            {
                int column = field.Coordinates.Column;
                int row = field.Coordinates.Row;
                Coordinates neighbourCoordinates = new Coordinates();
                switch(i)
                {
                    //On top
                    case 0:
                        neighbourCoordinates.Column = column;
                        neighbourCoordinates.Row = row - 1;
                        break;
                    //top right even/bottom right odd
                    case 1:
                        if (field.Coordinates.Column % 2 == 0)
                        {
                            neighbourCoordinates.Column = column + 1;
                            neighbourCoordinates.Row = row -1;
                            break;
                        }
                        else
                        {
                            neighbourCoordinates.Column = column + 1;
                            neighbourCoordinates.Row = row +1;
                            break;
                        }
                    //bottom right even/top right odd
                    case 2:
                        neighbourCoordinates.Column = column + 1;
                        neighbourCoordinates.Row = row;
                        break;
                    //bottom
                    case 3:
                        neighbourCoordinates.Column = column;
                        neighbourCoordinates.Row = row + 1;
                        break;
                    //bottom left even/top left odd
                    case 4:
                        neighbourCoordinates.Column = column - 1;
                        neighbourCoordinates.Row = row;
                        break;
                    //top left even/bottom left odd
                    case 5:
                        if (field.Coordinates.Column % 2 == 0)
                        {
                            neighbourCoordinates.Column = column - 1;
                            neighbourCoordinates.Row = row - 1;
                            break;
                        }
                        else
                        {
                            neighbourCoordinates.Column = column - 1;
                            neighbourCoordinates.Row = row + 1;
                            break;
                        }
                }

                if(IsCoordinateInRange(neighbourCoordinates) && GetField(neighbourCoordinates).FieldTerrain.TerrainID == Terrain.Ocean && !IsBorderCoordinate(neighbourCoordinates))
                {
                    neighbours.Add(neighbourCoordinates);
                }
            }
        }
        private void AddNeighbourTiles(Biome biome, ref List<List<Tile>> tiles, Coordinates coordinates)
        {
            for (int i = 0; i < 6; i++)
            {
                int column = coordinates.Column;
                int row = coordinates.Row;
                Coordinates neighbourCoordinates = new Coordinates();
                switch (i)
                {
                    //On top
                    case 0:
                        neighbourCoordinates.Column = column;
                        neighbourCoordinates.Row = row - 1;
                        break;
                    //top right even/bottom right odd
                    case 1:
                        if (coordinates.Column % 2 == 0)
                        {
                            neighbourCoordinates.Column = column + 1;
                            neighbourCoordinates.Row = row - 1;
                            break;
                        }
                        else
                        {
                            neighbourCoordinates.Column = column + 1;
                            neighbourCoordinates.Row = row + 1;
                            break;
                        }
                    //bottom right even/top right odd
                    case 2:
                        neighbourCoordinates.Column = column + 1;
                        neighbourCoordinates.Row = row;
                        break;
                    //bottom
                    case 3:
                        neighbourCoordinates.Column = column;
                        neighbourCoordinates.Row = row + 1;
                        break;
                    //bottom left even/top left odd
                    case 4:
                        neighbourCoordinates.Column = column - 1;
                        neighbourCoordinates.Row = row;
                        break;
                    //top left even/bottom left odd
                    case 5:
                        if (coordinates.Column % 2 == 0)
                        {
                            neighbourCoordinates.Column = column - 1;
                            neighbourCoordinates.Row = row - 1;
                            break;
                        }
                        else
                        {
                            neighbourCoordinates.Column = column - 1;
                            neighbourCoordinates.Row = row + 1;
                            break;
                        }
                }

                if (IsCoordinateInRange(neighbourCoordinates) && tiles[neighbourCoordinates.Column][neighbourCoordinates.Row].BiomeID == Biome.Ocean && !IsBorderCoordinate(neighbourCoordinates))
                {
                    biome.Neighbours.Add(neighbourCoordinates);
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
