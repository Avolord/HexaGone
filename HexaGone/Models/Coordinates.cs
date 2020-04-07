using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// A class describing the coordinates on the map in an offset coordinate system.
    /// </summary>
    public class Coordinates
    {
        //Attributes
        /// <summary>
        /// Describes the Column-Coordinate
        /// </summary>
        public double Column { get; set; }
        /// <summary>
        /// Describes the Row-Coordinate
        /// </summary>
        public double Row { get; set; }


        //Constructor
        /// <summary>
        /// Constructs Coordinates without specifying column and row.
        /// </summary>
        public Coordinates()
        { }
        /// <summary>
        /// Constructs Coordinates by specifying column and row.
        /// </summary>
        /// <param name="column">Describes the Column-Coordinate</param>
        /// <param name="row">Describes the Row-Coordinate</param>
        public Coordinates(double column, double row)
        {
            Column = column;
            Row = row;
        }

        //Functions
    }
}
