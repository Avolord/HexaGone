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
        public int MapSize { get; }
        public bool IsPointy { get; }
        public int HexSideLength { get; set; }
        public int Seed { get; set; }

        private Random rnd;
      

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
            //getting random seed
            Random s = new Random();
            Seed = s.Next(0, 2147483647);

            switch (sizeMode)
            {
                //Depanding on MapSize sets Width and Height.
                case Small:
                    Width = 100;
                    Height = 50;
                    break;
                case Medium:
                    Width = 200;
                    Height = 100;
                    break;
                case Big:
                    Width = 300;
                    Height = 160;
                    break;
                //sets the mapSize to  200 x 200
                default:
                    Width = 200;
                    Height = 100;
                    break;
            }
            MapSize = sizeMode;
            BiomeSize = biomeSize;
            IsPointy = false;
            HexSideLength = 30;
            Fields = new List<List<Field>>();
            rnd = new Random(Seed);
            System.Diagnostics.Debug.WriteLine("Seed: " + Seed);
            //Generate the Map
            MapgeneratorSwitcher(mapMode);
        }
        public Map(int mapMode, int sizeMode, int biomeSize, int seed)
        {
            switch (sizeMode)
            {
                //Depanding on MapSize sets Width and Height.
                case Small:
                    Width = 100;
                    Height = 50;
                    break;
                case Medium:
                    Width = 200;
                    Height = 100;
                    break;
                case Big:
                    Width = 300;
                    Height = 160;
                    break;
                //sets the mapSize to  200 x 200
                default:
                    Width = 200;
                    Height = 100;
                    break;
            }
            MapSize = sizeMode;
            BiomeSize = biomeSize;
            IsPointy = false;
            HexSideLength = 30;
            Fields = new List<List<Field>>();
            Seed = seed;
            rnd = new Random(Seed);
            System.Diagnostics.Debug.WriteLine("Seed: " + Seed);
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
            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for(int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for(int row = 0; row < Height; row++)
                {
                    Field field = new Field(column, row, rnd.Next(0, Terrain.amountTerrains));
                    columnList.Add(field);
                }
                //Add the columnList to Fields, otherwise everything was useless and the while-loop in the MapgeneratorSwitcher won't end.
                Fields.Add(columnList);
            }
        }
        private void TinyIslandsMapgenerator()
        {
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
                        field = new Field(column, row, rnd.Next(1, Terrain.amountTerrains));
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
                int randomIndex = rnd.Next(0, neighbours.Count);
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
        /// <summary>
        /// Creates a Map on base of random biomes
        /// </summary>
        private void BiomesMapgenerator()
        {
            //A 2-Dimensional List containing all the tiles with its biomes.
            List<List<Tile>> tiles = new List<List<Tile>>();
            //Create a List of all the biomes, that are created
            List<Biome> biomes = new List<Biome>();
            //Create a List with the biome probabilities. A biome that is more probable to be select, is more often in this list.
            List<int> biomeProbability = new List<int>();

            //Set a number of max Land Fields. In this case it's 40%

            int currentLandFields = GenerateBiomes(ref tiles, ref biomes, ref biomeProbability); ;
            int maxLandFields = Convert.ToInt32(Width*Height *0.6);

            //Select random neighbour fields and make them land
            while (currentLandFields <= maxLandFields)
            {
                //First select a random biome, that gets a new tile
                int randomBiomeIndex = biomeProbability[rnd.Next(0, biomeProbability.Count)];
                Biome randomBiome = biomes[randomBiomeIndex];

                //Check if the random biome has any neighbours
                if (randomBiome.Neighbours.Count > 0)
                {
                    //Select a random neighbour from its Neighbours List.
                    int randomIndex = rnd.Next(0, randomBiome.Neighbours.Count);
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

            CreateMountain(ref tiles);
            CreateHeightLevels(ref tiles);
            CoastlineGeneration(ref tiles, 2);

            //Translate Tilemap biomes into Terrains.
            //Create List of Fields with the Terrains from biomes.
            for (int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    int terrainID = GetTerrainIdFromBiomeIdAndHeight(tiles[column][row].BiomeID, tiles[column][row].Height);
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
            int landFields = 0;
            int amountBiomes = CreateAmountOfBiomes(); 

            //The first loop iterates over the Width, to create as many columns as needed, by creating a columnList.
            for (int column = 0; column < Width; column++)
            {
                List<Tile> columnList = new List<Tile>();
                //The second loop iterates over the Height, to create as many Tiles as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    //At first every tile-biome will be ocean
                    Tile tile = new Tile(column, row)
                    {
                        BiomeID = Biome.Ocean
                    };
                    columnList.Add(tile);
                }
                //Add the columnList to tiles.
                tiles.Add(columnList);
            }
            List<Coordinates> startPoints = SetStartPoints(amountBiomes);
            //This loop is for the creation of the biomes.
            for (int i = 0; i < amountBiomes; i++)
            {
                //Select the starting tile of the biome

                Coordinates coordinates = startPoints[i];
                Biome biome = GetBiomeFromHeight(Height, coordinates.Row);

                //Add the biome to the biomes List.
                biomes.Add(biome);
                landFields++;
                //Give the starting-Tile the biome ID.
                tiles[coordinates.Column][coordinates.Row].BiomeID = biome.ID;

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
        /// <summary>
        /// This methode sets the startpoints from the biomes based on the amount of biomes and Mapsize 
        /// </summary>
        /// <param name="amount">The amount of biomes</param>
        /// <returns>Returns a List with the Coordinates of the Startpoints</returns>
        private List<Coordinates> SetStartPoints(int amount)
        {
            //in this List the startingpoints are saved
            List<Coordinates> points = new List<Coordinates>();
            List<List<bool>> spots = new List<List<bool>>();
            int highest = 1;
            int pointsSet = 0;

            //Based on the Mapsize set the Startpoints
            switch (MapSize)
            {
                case 0:

                    for (int column = 0; column < Width / 10 -2; column++)
                    {
                        List<bool> columnList = new List<bool>();
                        //The second loop iterates over the Height, to create as many Tiles as there are Rows.
                        for (int row = 0; row < Height / 10 -2; row++)
                        {
                            //At first every tile-biome will be ocean

                            columnList.Add(false);
                        }
                        //Add the columnList to tiles.
                        spots.Add(columnList);
                    }
                    int[] smallCol = new int[8];
                    for (int i = 0; i<8; i++)
                    {
                        smallCol[i] = 0;
                    }
                    
                    while(pointsSet < amount)
                    {
                        Coordinates point = new Coordinates(rnd.Next(10, 90), rnd.Next(10, 40));
                        int[] temp = new int[2];
                        temp[0] = (point.Column / 10)-1;
                        temp[1] = (point.Row / 10)-1;

                        if(smallCol[temp[0]]<highest && !spots[temp[0]][temp[1]])
                        {
                            spots[temp[0]][temp[1]] = true;
                            smallCol[temp[0]] += 1;
                            pointsSet++;
                            points.Add(point);
                            if(pointsSet%8==0)
                            {
                                highest++;
                            }
                        }
                    }
                    break;

                case 1:

                    for (int column = 0; column < Width / 20 -2; column++)
                    {
                        List<bool> columnList = new List<bool>();
                        for (int row = 0; row < Height / 10 -4; row++)
                        {
                            columnList.Add(false);
                        }
                        spots.Add(columnList);
                    }

                    int[] mediumCol = new int[8];
                    for (int i = 0; i < 8; i++)
                    {
                        mediumCol[i] = 0;
                    }

                    while (pointsSet < amount)
                    {
                        Coordinates point = new Coordinates(rnd.Next(20, 180), rnd.Next(20, 80));
                        int[] temp = new int[2];
                        temp[0] = (point.Column / 20) - 1;
                        temp[1] = (point.Row / 10) - 2;

                        if (mediumCol[temp[0]] < highest && !spots[temp[0]][temp[1]])
                        {
                            spots[temp[0]][temp[1]] = true;
                            mediumCol[temp[0]] += 1;
                            pointsSet++;
                            points.Add(point);
                            if (pointsSet % 8 == 0)
                            {
                                highest++;
                            }
                        }
                    }
                    break;

                case 2:

                    for (int column = 0; column < Width / 20 - 2; column++)
                    {
                        List<bool> columnList = new List<bool>();
                        //The second loop iterates over the Height, to create as many Tiles as there are Rows.
                        for (int row = 0; row < Height / 20 - 2; row++)
                        {
                            //At first every tile-biome will be ocean

                            columnList.Add(false);
                        }
                        //Add the columnList to tiles.
                        spots.Add(columnList);
                    }

                    int[] BigCol = new int[13];
                    for (int i = 0; i < 13; i++)
                    {
                        BigCol[i] = 0;
                    }

                    while (pointsSet < amount)
                    {
                        Coordinates point = new Coordinates(rnd.Next(20, 280), rnd.Next(20, 140));
                        int[] temp = new int[2];
                        temp[0] = (point.Column / 20)-1;
                        temp[1] = (point.Row / 20)-1;

                        if (BigCol[temp[0]] < highest && !spots[temp[0]][temp[1]])
                        {
                            spots[temp[0]][temp[1]] = true;
                            BigCol[temp[0]] += 1;
                            pointsSet++;
                            points.Add(point);
                            if (pointsSet % 13 == 0)
                            {
                                highest++;
                            }
                        }
                    }
                    break;
            }

            return points;
        }
        /// <summary>
        /// This methode creates an integer based on Mapsize and Biomesize which is used for the Amount of Biomes that should be created 
        /// </summary>
        /// <returns>Amount of Biomes for the Map</returns>
        private int CreateAmountOfBiomes()
        {
            int amount = 0;

            //This switch case decides how much Biomes will be generated for the map
            switch(MapSize)
            {
                case 0:
                    switch (BiomeSize)
                    {
                        case 0:
                            amount = rnd.Next(19, 23);
                            
                            break;
                        case 1:
                            amount = rnd.Next(15, 19);
                            
                            break;
                        case 2:
                            amount = rnd.Next(9, 15);
                            
                            break;
                        default:
                            amount = rnd.Next(15, 19);
                            
                            break;
                    }
                    break;
                case 1:
                    switch (BiomeSize)
                    {
                        case 0:
                            amount = rnd.Next(35, 45);
                            
                            break;
                        case 1:
                            amount = rnd.Next(28, 35);
                            
                            break;
                        case 2:
                            amount = rnd.Next(20, 28);
                            
                            break;
                        default:
                            amount = rnd.Next(28, 35);
                            
                            break;
                    }
                    break;
                case 2:
                    switch (BiomeSize)
                    {
                        case 0:
                            amount = rnd.Next(65, 78);
                            
                            break;
                        case 1:
                            amount = rnd.Next(45, 65);
                            
                            break;
                        case 2:
                            amount = rnd.Next(30, 45);
                            
                            break;
                        default:
                            amount = rnd.Next(40, 50);
                            
                            break;
                    }
                    break;
                default:
                    switch (BiomeSize)
                    {
                        case 0:
                            amount = rnd.Next(40, 45);
                            
                            break;
                        case 1:
                            amount = rnd.Next(30, 40);
                            
                            break;
                        case 2:
                            amount = rnd.Next(20, 30);
                            
                            break;
                        default:
                            amount = rnd.Next(30, 40);
                            
                            break;
                    }
                    break;
            }

            return amount;
        }
        private void CoastlineGeneration(ref List<List<Tile>> tiles, int coastWidth)
        {

            foreach (List<Tile> tileColumn in tiles)
            {
                foreach (Tile tile in tileColumn)
                {
                    if (tile.BiomeID == Biome.Lake)
                    {
                        tile.BiomeID = Biome.Ocean;
                    }
                }
            }

            for (int i = 0; i < coastWidth; i++)
            {
                List<Tile> newCoastTiles = new List<Tile>();
                foreach (List<Tile> tileColumn in tiles)
                {
                    foreach (Tile tile in tileColumn)
                    {
                        if (tile.BiomeID == Biome.Ocean && IsCoastTile(ref tiles, tile.Coordinates))
                        {
                            newCoastTiles.Add(tile);
                        }
                    }
                }
                foreach(Tile tile in newCoastTiles)
                {
                    tile.BiomeID = Biome.Lake;
                }
            }

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
        private int GetTerrainIdFromBiomeIdAndHeight(int biomeId, int height)
        {
            int veryHigh = 70;
            int high = 30;
            int low = 10;

            switch (biomeId)
            {
                case Biome.Plains:
                    if(height >= veryHigh)
                    {
                        return Terrain.Mountain;
                    }
                    else if(height >= high)
                    {
                        return Terrain.Hills;
                    }
                    if(rnd.Next(0,3) < 2)
                    {
                        return Terrain.Plains;
                    }
                    else
                    {
                        return Terrain.Trees;
                    }


                case Biome.Forest:
                    if (height >= veryHigh)
                    {
                        return Terrain.Mountain;
                    }
                    else if (height >= high)
                    {
                        return Terrain.HillTrees;
                    }

                    int probabilityForest = 6;
                    int probabilityTrees = 2;
                    int probabilityPlains = 1;

                    int randomTerrain = rnd.Next(0, probabilityForest + probabilityPlains + probabilityTrees);
                    if(randomTerrain < probabilityForest)
                    {
                        return Terrain.Forest;
                    }
                    else if(randomTerrain < probabilityForest + probabilityTrees)
                    {
                        return Terrain.Trees;
                    }
                    else
                    {
                        return Terrain.Plains;
                    }
                    


                case Biome.Desert:
                    if (height >= veryHigh)
                    {
                        return Terrain.DesertMountain;
                    }
                    else if (height >= high)
                    {
                        return Terrain.DesertHills;
                    }

                    if (rnd.Next(0, 3) < 2)
                    {
                        return Terrain.DesertPlains;
                    }
                    else
                    {
                        return Terrain.DesertDunes;
                    }


                case Biome.Lake:
                    return Terrain.ShallowWater;


                case Biome.Jungle:
                    if (height >= veryHigh)
                    {
                        return Terrain.Mountain;
                    }
                    return Terrain.Jungle;


                case Biome.Swamp:
                    if (height >= veryHigh)
                    {
                        return Terrain.Mountain;
                    }
                    else if (height >= high)
                    {
                        return Terrain.SwampTrees;
                    }
                    else if(height >= low)
                    {
                        return Terrain.SwampPlains;
                    }
                    return Terrain.SwampPond;


                case Biome.Tundra:
                    if (height >= veryHigh)
                    {
                        return Terrain.Mountain;
                    }
                    else if (height >= high)
                    {
                        return Terrain.SnowHillTrees;
                    }
                    if (rnd.Next(0, 3) < 2)
                    {
                        return Terrain.SnowTrees;
                    }
                    else
                    {
                        return Terrain.SnowPlains;
                    }


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
        private bool IsCoastTile(ref List<List<Tile>> tiles, Coordinates coordinates)
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

                if (IsCoordinateInRange(neighbourCoordinates) && tiles[neighbourCoordinates.Column][neighbourCoordinates.Row].BiomeID != Biome.Ocean)
                {
                    return true;
                }
            }
            return false;
        }
        public Biome GetBiomeFromHeight(int mapHeight, int tileHeight)
        {
            double heightPercentage = Convert.ToDouble(tileHeight) / Convert.ToDouble(mapHeight);

            List<int> biomeProbabilities = new List<int>();

            //Add standard biomes
            for (int i = 0; i < 10; i++)
            {
                biomeProbabilities.Add(Biome.Plains);
                biomeProbabilities.Add(Biome.Forest);
                biomeProbabilities.Add(Biome.Lake);
                biomeProbabilities.Add(Biome.Swamp);
                biomeProbabilities.Add(Biome.Jungle);
            }

            if (heightPercentage < 0.4)
            {
                int amountTundra = Convert.ToInt32(-24.21448 + (147.6358 - -24.21448) / (1 + Math.Pow((heightPercentage / 0.1724576), 1.755318)));
                for (int i = 0; i < amountTundra; i++)
                {
                    biomeProbabilities.Add(Biome.Tundra);
                }
            }

            if (heightPercentage > 0.6)
            {
                int amountDesert = Convert.ToInt32(129.6022 + (-3.675622 - 129.6022) / (1 + Math.Pow((heightPercentage / 0.8425428), 7.299509)));
                for (int i = 0; i < amountDesert; i++)
                {
                    biomeProbabilities.Add(Biome.Desert);
                }
            }

            int randomBiome = biomeProbabilities[rnd.Next(0, biomeProbabilities.Count)];

            Biome biome = new Biome(randomBiome);
            return biome;
        }
        private void CreateMountain(ref List<List<Tile>> tiles)
        {
            int amountMountains = (MapSize + 1) * 33;

            for (int mountain = 0; mountain < amountMountains; mountain++)
            {
                //Startpunkt Gebirge
                Coordinates latestPoint = new Coordinates(rnd.Next(1, Width - 1), rnd.Next(1, Height - 1));
                //Richtung Gebirge
                int direction = rnd.Next(0, 6);
                //Länge Gebirge
                int length = rnd.Next(10, 50);
                //Höhe des Gebirges
                int height = rnd.Next(50, 90);
                //Wie stark das Gebirge es vorzieht in die gegebene Richtung zu gehen.
                int straightness = 10;
                //Liste in der die ^Koordinaten gespeichert werden, welche als nächstes ausgewählt werden können
                List<Coordinates> coordinateProbabilities = new List<Coordinates>();

                //Höhe des Startpunktes anpassen
                while (tiles[latestPoint.Column][latestPoint.Row].BiomeID == Biome.Ocean || tiles[latestPoint.Column][latestPoint.Row].BiomeID == Biome.Lake)
                {
                    latestPoint = new Coordinates(rnd.Next(1, Width - 1), rnd.Next(1, Height - 1));
                }
                tiles[latestPoint.Column][latestPoint.Row].Height = rnd.Next(height - 5, height + 6);
                //tiles[latestPoint.Column][latestPoint.Row].BiomeID = 8;
                //Für die Länge des Gebirges werden neu Tiles ausgewählt
                for (int i = 0; i < length; i++)
                {
                    bool noNeighbour = false;
                    switch (direction)
                    {
                        //top
                        case 0:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetTopNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopLeftNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopRightNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetBottomLeftNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetBottomRightNeighbour(latestPoint));

                            break;
                        //top right
                        case 1:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetTopRightNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomRightNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetTopLeftNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetBottomNeighbour(latestPoint));

                            break;
                        //bottom right
                        case 2:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetBottomRightNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopRightNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetBottomLeftNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetTopNeighbour(latestPoint));

                            break;
                        //bottom
                        case 3:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetBottomNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomRightNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomLeftNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetTopLeftNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetTopRightNeighbour(latestPoint));

                            break;
                        //bottom left
                        case 4:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetBottomLeftNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopLeftNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetBottomRightNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetTopNeighbour(latestPoint));
                            break;
                        //up left
                        case 5:
                            for (int n = 0; n < straightness; n++)
                            {
                                coordinateProbabilities.Add(GetTopLeftNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetBottomLeftNeighbour(latestPoint));
                                coordinateProbabilities.Add(GetTopNeighbour(latestPoint));
                            }
                            coordinateProbabilities.Add(GetBottomNeighbour(latestPoint));
                            coordinateProbabilities.Add(GetTopRightNeighbour(latestPoint));
                            break;
                    }

                    int neighbourIndex = rnd.Next(0, coordinateProbabilities.Count);
                    latestPoint = coordinateProbabilities[neighbourIndex];

                    while ((tiles[latestPoint.Column][latestPoint.Row].BiomeID == Biome.Ocean || tiles[latestPoint.Column][latestPoint.Row].BiomeID == Biome.Lake) && coordinateProbabilities.Count != 0)
                    {
                        coordinateProbabilities.RemoveAt(neighbourIndex);

                        if (coordinateProbabilities.Count > 0)
                        {
                            neighbourIndex = rnd.Next(0, coordinateProbabilities.Count);
                            latestPoint = coordinateProbabilities[neighbourIndex];
                        }
                        else
                        {
                            noNeighbour = true;
                        }

                    }

                    if (noNeighbour)
                    {
                        break;
                    }

                    if (IsCoordinateInRange(latestPoint))
                    {
                        int newHeight = rnd.Next(height - 10, height + 11);
                        if (newHeight > tiles[latestPoint.Column][latestPoint.Row].Height)
                        {
                            tiles[latestPoint.Column][latestPoint.Row].Height = newHeight;
                        }
                        //tiles[latestPoint.Column][latestPoint.Row].BiomeID = 8;
                    }
                    coordinateProbabilities.Clear();
                }
            }
        }
        /// <summary>
        /// This Methode returns the Top Right Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Top Right Neigbour</returns>
        private Coordinates GetTopRightNeighbour(Coordinates point)
        {
            Coordinates neighbour;

            if(point.Column %2 == 0)
            {
                neighbour = new Coordinates(point.Column + 1, point.Row - 1);
            }
            else
            {
                neighbour = new Coordinates(point.Column + 1, point.Row);
            }

            return neighbour;
        }
        /// <summary>
        /// This Methode returns the Bottom Right Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Bottom Right Neigbour</returns>
        private Coordinates GetBottomRightNeighbour(Coordinates point)
        {
            Coordinates neighbour;

            if (point.Column % 2 == 0)
            {
                neighbour = new Coordinates(point.Column + 1, point.Row);
            }
            else
            {
                neighbour = new Coordinates(point.Column + 1, point.Row + 1);
            }

            return neighbour;
        }
        /// <summary>
        /// This Methode returns the Top Left Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Top Left Neigbour</returns>
        private Coordinates GetTopLeftNeighbour(Coordinates point)
        {
            Coordinates neighbour;

            if (point.Column % 2 == 0)
            {
                neighbour = new Coordinates(point.Column - 1, point.Row - 1);
            }
            else
            {
                neighbour = new Coordinates(point.Column - 1, point.Row);
            }

            return neighbour;
        }
        /// <summary>
        /// This Methode returns the Bottom Left Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Bottom Left Neigbour</returns>
        private Coordinates GetBottomLeftNeighbour(Coordinates point)
        {
            Coordinates neighbour;

            if (point.Column % 2 == 0)
            {
                neighbour = new Coordinates(point.Column - 1, point.Row);
            }
            else
            {
                neighbour = new Coordinates(point.Column - 1, point.Row +1);
            }

            return neighbour;
        }
        /// <summary>
        /// This Methode returns the Top Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Top Neigbour</returns>
        private Coordinates GetTopNeighbour(Coordinates point)
        {
            return new Coordinates(point.Column, point.Row - 1);
        }
        /// <summary>
        /// This Methode returns the Bottom Neigbour of a given Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Bottom Neigbour</returns>
        private Coordinates GetBottomNeighbour(Coordinates point)
        {
            return new Coordinates(point.Column, point.Row + 1);
        }
        private void CreateHeightLevels(ref List<List<Tile>> tiles)
        {
            //the amount of height a tile will get less than its neighbours
            int reduction = 15;
            //sets the amount a tile can go up and down with the height randomly
            int spread = 5;

            //This integer counts the highest height set in one iteration of the while loop. If it is too low, the loop won't continue.
            int highestHeight = 11;

            while (highestHeight > 10)
            {
                highestHeight = 0;

                //Create two lists. One with the tiles that need to be changed and one with the new heights they are getting.
                List<Tile> newHeightsTiles = new List<Tile>();
                List<int> newHeights = new List<int>();

                foreach (List<Tile> tileColumn in tiles)
                {
                    foreach (Tile tile in tileColumn)
                    {
                        //Only land tiles that don't have a height yet, get a new height
                        if ((tile.BiomeID != Biome.Ocean || tile.BiomeID != Biome.Lake) && tile.Height == 0)
                        {
                            int newHeight = 0;
                            int amountNeighbourHeights = 0;

                            //this loop goes through every neighbour and checks if the Height is bigger than 0, so the Height can be added to newHeight
                            for (int i = 0; i < 6; i++)
                            {
                                Coordinates neighbourCoordinates;
                                switch (i)
                                {
                                    case 0:
                                        neighbourCoordinates = GetTopNeighbour(tile.Coordinates);
                                        break;
                                    case 1:
                                        neighbourCoordinates = GetTopRightNeighbour(tile.Coordinates);
                                        break;
                                    case 2:
                                        neighbourCoordinates = GetBottomRightNeighbour(tile.Coordinates);
                                        break;
                                    case 3:
                                        neighbourCoordinates = GetBottomNeighbour(tile.Coordinates);
                                        break;
                                    case 4:
                                        neighbourCoordinates = GetBottomLeftNeighbour(tile.Coordinates);
                                        break;
                                    case 5:
                                        neighbourCoordinates = GetTopLeftNeighbour(tile.Coordinates);
                                        break;
                                    //There is no default case in this loop.
                                    default:
                                        neighbourCoordinates = GetTopNeighbour(tile.Coordinates);
                                        break;
                                }

                                if (IsCoordinateInRange(neighbourCoordinates) && tiles[neighbourCoordinates.Column][neighbourCoordinates.Row].Height > 0)
                                {
                                    newHeight += tiles[neighbourCoordinates.Column][neighbourCoordinates.Row].Height;
                                    amountNeighbourHeights += 1;
                                }
                            }

                            //If the tile has neighbours with height, it gets added to the two lists from above.
                            if (amountNeighbourHeights > 0)
                            {
                                newHeight /= amountNeighbourHeights;
                                newHeightsTiles.Add(tile);
                                newHeights.Add(newHeight);
                            }
                        }
                    }
                }

                //Go through the two lists and add the heights to the tiles
                for (int i = 0; i < newHeights.Count; i++)
                {
                    int newHeight = newHeights[i] - reduction + rnd.Next(-spread, spread +1);
                    newHeightsTiles[i].Height = newHeight;

                    if(newHeight > highestHeight )
                    {
                        highestHeight = newHeight;
                    }
                }
            }
        }
    }
}