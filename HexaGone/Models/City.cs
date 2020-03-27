using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// A City which can be build on a Hexagon
    /// </summary>
    public class City : Construction
    {   //Attributes
        /// <summary>
        /// A Uniqe ID for every City
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// The influence size of a city
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// The Resource stock of the city
        /// </summary>
        public Resource[] Resources { get; set; }
        /// <summary>
        /// Size of The Population in the city
        /// </summary>
        public int Population { get; set; }
        /// <summary>
        /// Field on which the city is build
        /// </summary>
        public Field ParrentField { get; set; }

        //Constructor
        /// <summary>
        /// Constructs a City from Parrent Field
        /// </summary>
        /// <param name="_Parrent">The Parrent Field</param>
        /// <param name="_Size">The influence size of the city. Optional. Defaults to 20.</param>
        /// <param name="_Population">The Population of a city. Optional. Defaults to 1000.</param>
        /// <param name="_Resources">The Resource stock of the city. Optional. Defaults to 0.</param>
        public City(Field _Parrent, int _Size = 20, int _Population = 1000, Resource[] _Resources = null)
        {
            ParrentField = _Parrent;
            CityID = CreateUniqueCityID();
            Size = _Size;
            Population = _Population;

            if(_Resources != null)
            {
                Resources = _Resources;
            }
            else
            {
                CreateResource();
            }

        }
        public int CreateUniqueCityID()
        {
            //TO ADD: Everything.
            return 0;
        }
        
        private Resource[] CreateResource()
        {
            Resource[] stock = new Resource[6];
            stock[0] = new Resource(0, 100);
            for (int i = 1; i<5; i++)
            {
                stock[i] = new Resource(i, 1000);
            }
            return stock;
        }
    }
}
