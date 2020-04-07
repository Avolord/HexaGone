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
        public const int mapModeOneBigIsland = 2;

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

                    case mapModeOneBigIsland:
                        OneBigIslandMapgenerator();
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

        private void OneBigIslandMapgenerator()
        {
            var gen = MapGen(1.5);
            for (int column = 0; column < Width; column++)
            {
                List<Field> columnList = new List<Field>();
                //The second loop iterates over the Height, to create as many Fields as there are Rows.
                for (int row = 0; row < Height; row++)
                {
                    Coordinates q = new Coordinates(column/50, row/50);
                    Field field;
                    bool isLand = gen(q);
                    if(isLand)
                    {
                        field = new Field(column, row, Terrain.terrainIdDesert);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="island_Factor">1.0 means no small islands; 2.0 leads to a lot</param>
        /// <param name="seed">Optional. Seed to generate world</param>
        /// <returns></returns>
        private Func<Coordinates, bool> MapGen(double island_Factor, int seed = -1)
        {
            double ISLAND_FACTOR = island_Factor;
            Random rnd;
            if (seed == -1)
            {
                rnd = new Random();
            }
            else
            {
                rnd = new Random(seed);
            }
            
            int bumps = rnd.Next(1, 6);
            double startAngle = 0 + ((2 * Math.PI -0) * rnd.NextDouble());
            double dipAngle = 0 + ((2 * Math.PI - 0) * rnd.NextDouble());
            double dipWidth = 0.2 + ((0.7 - 0.2) * rnd.NextDouble());

            bool inside(Coordinates q)
            {
                double angle = Math.Atan2(q.Row, q.Column);
                double length = 0.5 * (Math.Max(Math.Abs(q.Column), Math.Abs(q.Row)) + Math.Sqrt((q.Row*q.Row)+(q.Column*q.Column)));

                double r1 = 0.5 + 0.40 * Math.Sin(startAngle + bumps * angle + Math.Cos((bumps + 3) * angle));
                double r2 = 0.7 - 0.20 * Math.Sin(startAngle + bumps * angle - Math.Sin((bumps + 2) * angle));

                if (Math.Abs(angle-dipAngle)< dipWidth || Math.Abs(angle - dipAngle + 2 * Math.PI) < dipWidth || Math.Abs(angle - dipAngle - 2 * Math.PI) < dipWidth)
                {
                    r1 = r2 = 0.2;
                }
                return (length < r1 || (length > (r1*ISLAND_FACTOR) && length <r2));
            }

            return inside;
        }
    }
}
