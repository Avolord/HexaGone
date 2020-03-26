using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class Resource
    {
        //Attributes
        /// <summary>
        /// The ID of the resource.
        /// </summary>
        public int ResourceID { get; set; }
        /// <summary>
        /// The name of the resource.
        /// </summary>
        public string Name { get; set; }

        //Static Attributes

        public const int resourceID_Gold = 0;
        public const int resourceID_Wood = 1;
        public const int resourceID_Food = 2;
        public const int resourceID_Stone = 3;
        public const int resourceID_Copper = 4;
        public const int resourceID_Iron = 5;


        //Constructor

        /// <summary>
        /// Constructs a Resource from a _ResourceID. Currently supports IDs ranging from 0-5.
        /// </summary>
        /// <param name="_ResourceID">
        /// 0 = Gold ;
        /// 1 = Wood ;
        /// 2 = Food ;
        /// 3 = Stone ;
        /// 4 = Copper ;
        /// 5 = Iron ;
        /// defaults to Gold
        /// </param>
        public Resource(int _ResourceID)
        {
            ResourceID = _ResourceID;

            switch(ResourceID)
            {
                //Gold
                case resourceID_Gold:
                    Name = "Gold";
                    break;
                //Wood
                case resourceID_Wood:
                    Name = "Wood";
                    break;
                //Food
                case resourceID_Food:
                    Name = "Food";
                    break;
                //Stone
                case resourceID_Stone:
                    Name = "Stone";
                    break;
                //Copper
                case resourceID_Copper:
                    Name = "Copper";
                    break;
                //Iron
                case resourceID_Iron:
                    Name = "Iron";
                    break;
                //Default (in this case Gold)
                default:
                    ResourceID = resourceID_Gold;
                    Name = "Gold";
                    break;

            }
        }
        //Functions
    }
}
