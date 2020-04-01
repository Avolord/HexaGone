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
        /// <summary>
        /// The Amount of this Resource
        /// </summary>
        public int Amount { get; set; }
        //Static Attributes

        public const int resourceIdGold = 0;
        public const int resourceIdWood = 1;
        public const int resourceIdFood = 2;
        public const int resourceIdStone = 3;
        public const int resourceIdCopper = 4;
        public const int resourceIdIron = 5;


        //Constructor

        /// <summary>
        /// Constructs a Resource from a _ResourceID. Currently supports IDs ranging from 0-5.
        /// </summary>
        /// <param name="resourceID">
        /// 0 = Gold ;
        /// 1 = Wood ;
        /// 2 = Food ;
        /// 3 = Stone ;
        /// 4 = Copper ;
        /// 5 = Iron ;
        /// defaults to Gold
        /// </param>
        /// <param name="amount">The amount of this Resource. Optional. Defaults to 0</param>
        public Resource(int resourceID, int amount = 0)
        {
            ResourceID = resourceID;
            Amount = amount;

            switch (ResourceID)
            {
                //Gold
                case resourceIdGold:
                    Name = "Gold";
                    break;
                //Wood
                case resourceIdWood:
                    Name = "Wood";
                    break;
                //Food
                case resourceIdFood:
                    Name = "Food";
                    break;
                //Stone
                case resourceIdStone:
                    Name = "Stone";
                    break;
                //Copper
                case resourceIdCopper:
                    Name = "Copper";
                    break;
                //Iron
                case resourceIdIron:
                    Name = "Iron";
                    break;
                //Default (in this case Gold)
                default:
                    ResourceID = resourceIdGold;
                    Name = "Gold";
                    break;

            }
        }

        //Functions
    }
}
