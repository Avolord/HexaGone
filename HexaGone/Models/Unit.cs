using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public enum UnitClass
    {
        SwordAndShield,
        SpearAndShield,
        Pike,
        Archer,
        Cavalry,
        Strike
    }

    public class Unit
    {
        //Attributes
        /// <summary>
        /// A unique ID for the Unit.
        /// </summary>
        public int UnitID { get; set; }
        /// <summary>
        /// A ID that specifies the class of the Unit.
        /// </summary>
        public UnitClass Class { get; set; }
        /// <summary>
        /// The name of the unit.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Attack Stat of the unit.
        /// </summary>
        public float AttackStat { get; set; }
        /// <summary>
        /// The Defense Stat of the unit.
        /// </summary>
        public float DefenseStat { get; set; }
        /// <summary>
        /// The maximum number of men in the Unit.
        /// </summary>
        public int MaxMenCount { get; set; }
        /// <summary>
        /// The current number of men in the Unit.
        /// </summary>
        public int MenCount { get; set; }
        /// <summary>
        /// Is true if the Unit is a ranged Unit.
        /// </summary>
        public bool IsRangedUnit { get; set; }
        /// <summary>
        /// This Dictionary lists all multipliers this Unit gets against the other UnitClasses.
        /// </summary>
        public Dictionary<int, float> Factors { get; }
        /// <summary>
        /// Indicates how far the Unit can move in one turn.
        /// </summary>
        public float MovementPoints { get; set; }
    }
}
