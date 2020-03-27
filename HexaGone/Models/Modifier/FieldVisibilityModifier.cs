﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    public class FieldVisibilityModifier : FieldModifier
    {
        //Attributes


        //Constants
        public const int modifier_Terrain = 0;

        private const int modifier_ExampleAdditiveModifier = 42;

        //Constructor

        /// <summary>
        /// Creates a new FieldExpendituresModifier.
        /// </summary>
        /// <param name="_ParentField">The Field this modifier is applied to.</param>
        /// <param name="_ModifierID">The modifier ID. Is used so a field can't have multiple modifiers with the same ID. Can still share the same ID with other field modifier types.</param>
        /// <param name="_Value">Optional. Sets the Value of the modifier. Default is 1.</param>
        public FieldVisibilityModifier(Field _ParentField, int _ModifierID, double _Value = 1)
        {
            ParentField = _ParentField;
            ModifierID = _ModifierID;
            Value = _Value;

            //A switch-case statement about specific modifiers. Not every modifier needs a case here. But definitely every additive modifier.
            switch (ModifierID)
            {
                //Terrain-Modifier
                case modifier_Terrain:
                    Name = ParentField.FieldTerrain.Name + " Modifier";
                    Value = ParentField.FieldTerrain.BaseVisibility;
                    break;

                //An example modifier that shows how an additive modifier is added. It simply sets the Additive boolean to true.
                //Has 0 as a Value as not to change anything ingame, if it is actually applied accidentally.
                case 42:
                    Name = "EXAMPLE MODIFIER. SHOULD NOT BE INGAME. SORRY.";
                    Value = 0;
                    Additive = true;
                    break;

                //No case - Do nothing
                default:
                    break;
            }
        }
        //Functions
    }
}
