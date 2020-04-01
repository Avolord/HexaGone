using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HexaGone.Models
{
    /// <summary>
    /// A City which can be build on a Hexagon
    /// </summary>
    public class City : Construction
    {   //Attributes
        /// <summary>
        /// A Uniqe ID for the City
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// The name of the City. Is given by the user.
        /// </summary>
        [Required]
        [Display(Name = "City Name")]
        [StringLength(25, MinimumLength = 3)]
        public string CityName { get; set; }
        /// <summary>
        /// The Resource stock of the city
        /// </summary>
        public ResourceList Resources { get; set; }
        /// <summary>
        /// The Influence of the City.
        /// </summary>
        public int Influence { get; set; }

        //TODO: Add a List of Jobs
        /// <summary>
        /// Size of The Population in the city
        /// </summary>
        public int Population { get; set; }
        /// <summary>
        /// Field on which the city is build
        /// </summary>
        public Field ParentField { get; set; }
        /// <summary>
        /// A List of Fields that belong to the city.
        /// </summary>
        public List<Field> Fields { get; }
        /// <summary>
        /// A List of Buildings built in the city.
        /// </summary>
        public List<CityBuilding> CityBuildings { get; }
        //Are those even necessary with the List of Fields?
        /// <summary>
        /// A List of FieldBuildings built in the influence area of the city.
        /// </summary>
        public List<FieldBuilding> FieldBuildings { get; }

        //Constants

        /// <summary>
        /// The standard starting Influence of a city. 
        /// </summary>
        public const int StartingInfluence = 18;
        /// <summary>
        /// The standard starting Population of a city.
        /// </summary>
        public const int StartingPopulation = 1000;
        /// <summary>
        /// The standard stating Jobs of a city.
        /// </summary>
        public const int StartingJobs = 100;

        /// <summary>
        /// The standard cost of a city.
        /// </summary>
        public static readonly ResourceList StandardCityCost = new ResourceList()
        {
            Gold = 100,
            Wood = 1000,
            Food = 1000,
            Stone = 1000,
            Copper = 1000,
            Iron = 1000
        };
        /// <summary>
        /// The standard starting Stock of a city.
        /// </summary>
        public static readonly ResourceList StandardCityStock = new ResourceList()
        {
            Gold = 100,
            Wood = 1000,
            Food = 1000,
            Stone = 1000,
            Copper = 1000,
            Iron = 1000
        };


        //Constructor
        /// <summary>
                /// Constructs a City from Parent Field
                /// </summary>
                /// <param name="_Parent">The Parrent Field</param>
                /// <param name="_Influence">The influence size of the city. Optional. Defaults to 20.</param>
                /// <param name="_Population">The Population of a city. Optional. Defaults to 1000.</param>
                /// <param name="_Resources">The Resource stock of the city. Optional. Defaults to 0.</param>
        public City(Field _Parent, int _Influence = StartingInfluence, int _Population = StartingPopulation, ResourceList _Resources = null)
        {
            ParentField = _Parent;
            CityID = CreateUniqueCityID();
            Influence = _Influence;
            Population = _Population;
            Jobs = StartingJobs;
            Name = "City";
            NeedsField = true;
            Cost = StandardCityCost;
            
            //Initialise Lists
            Fields = new List<Field>();
            CityBuildings = new List<CityBuilding>();
            FieldBuildings = new List<FieldBuilding>();

            if(_Resources != null)
            {
                Resources = _Resources;
            }
            else
            {
                Resources = StandardCityStock;
            }

        }

        //Functions

        /// <summary>
        /// Creates a unique ID for the City, with help from its coordinates.
        /// </summary>
        /// <returns>A unique City ID</returns>
        private int CreateUniqueCityID()
        {
            //TO ADD: Everything.
            return 0;
        }

        public void BuildCityBuilding(int CityBuildingID)
        {
            if (true)//TODO: Check if there is enough space and the resources it requires are there.
            {
                //TODO: Add amount of time it needs to build a building
                CityBuildings.Add(new CityBuilding(this, CityBuildingID));
            }
        }
        //TODO: Is this function even required here in the City class or elsewhere?
        /// <summary>
        /// Builds a FieldBuilding in the influence area of this city.
        /// </summary>
        public void BuildFieldBuilding(Field field, int FieldBuildingID)
        {
            //TODO: These checks:
            //Is the Field free?
            //Can the building be build there?
            //Are the resource requirements met?
            if(true)
            {
                FieldBuildings.Add(new FieldBuilding(this, field, FieldBuildingID));
            }
        }
        
    }
}
