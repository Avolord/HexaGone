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
        /// The Resource stock of the city
        /// </summary>
        public List<Resource> Resources { get; set; }
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
        /// <param name="_Influence">The influence size of the city. Optional. Defaults to 20.</param>
        /// <param name="_Population">The Population of a city. Optional. Defaults to 1000.</param>
        /// <param name="_Resources">The Resource stock of the city. Optional. Defaults to 0.</param>
        public City(Field _Parrent, int _Influence = 20, int _Population = 1000, List<Resource> _Resources = null)
        {
            ParrentField = _Parrent;
            CityID = CreateUniqueCityID();
            Influence = _Influence;
            Population = _Population;
            Jobs = 100;
            Name = "City";
            Cost = setCost();
            if(_Resources != null)
            {
                Resources = _Resources;
            }
            else
            {
                Resources = Resource.createStock(100, 1000, 1000, 1000, 1000, 1000);
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

        private Resource[] setCost()
        {
            Resource[] cost = new Resource[6];
            cost[0] = new Resource(0, 1000);
            cost[1] = new Resource(0, 1000);
            cost[2] = new Resource(0, 1000);
            cost[3] = new Resource(0, 1000);
            cost[4] = new Resource(0, 1000);
            cost[5] = new Resource(0, 1000);
            return cost;
        }
    }
}
