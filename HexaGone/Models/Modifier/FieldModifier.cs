using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    public class FieldModifier
    {
        /// <summary>
        /// ID for the modifier. Is used to not have the a modifier od the same ID multiple times on a Field.
        /// </summary>
        public int ModifierID { get; set; }
        /// <summary>
        /// The name of the modifier as displayed to the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Value of the modifier. A value that does nothing is for multiplicative (Additive==false) modifiers 1 and for additive modifiers 0.
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// The Field the modofier is applied to.
        /// </summary>
        public Field ParentField { get; set; }
        /// <summary>
        /// Is an additive modifier if true and a multiplier if false.
        /// </summary>
        public bool Additive { get; set; }
    }
}
