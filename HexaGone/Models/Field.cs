using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    /// <summary>
    /// One hexagon Field in the world.
    /// </summary>
    public class Field
    {
        //Attributes
        /// <summary>
        /// A unique ID for the Field.
        /// </summary>
        public int FieldID { get; set; }
        /// <summary>
        /// The coordinates on the Map. Are given at initialisation.
        /// </summary>
        public int[] Coordinates { get; set; }
        /// <summary>
        /// The Terrain at this Field. Is given at initialisation.
        /// </summary>
        public Terrain FieldTerrain { get; set; }
        /// <summary>
        /// The height of this Field. Can be given at initialisation.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The Empire this Field belongs to. Can be null if it doesnt belong to any.
        /// </summary>
        public Empire ParentEmpire { get; set; }
        /// <summary>
        /// The City this Field belongs to. Can be null if it doesnt belong to any.
        /// </summary>
        public City ParentCity { get; set; }
        /// <summary>
        /// The Construction on this Field. It cant have a FieldBuilding if it is a city and vice versa.
        /// </summary>
        public Construction Construction { get; set; }
        /// <summary>
        /// A list of the available resources on this Field.
        /// </summary>
        public List<int> FieldResources { get; }
        //TO ADD: Streets
        /// <summary>
        /// The factor of the speed at which entities can pass through the terrain.
        /// </summary>
        public double MovementRate { get; set; }
        /// <summary>
        /// A List of all MovementRateModifiers affecting this field. Is sorted by the Additive boolean value. First all Additive modifiers and then all !Additive ones.
        /// </summary>
        public List<Modifier.FieldMovementRateModifier> MovementRateModifiers { get; }
        /// <summary>
        /// The factor of the visibility. The higher, the better an entity can see into and through the field.
        /// </summary>
        public double Visibility { get; set; }
        /// <summary>
        /// A List of all VisibilityModifiers affecting this field. Is sorted by the Additive boolean value. First all Additive modifiers and then all !Additive ones.
        /// </summary>
        public List<Modifier.FieldVisibilityModifier> VisibilityModifiers { get; }
        /// <summary>
        /// The factor of the food/gold expenditures for resting armies.
        /// </summary>
        public double Expenditures { get; set; }
        /// <summary>
        /// A List of all ExpendituresModifiers affecting this field. Is sorted by the Additive boolean value. First all Additive modifiers and then all !Additive ones.
        /// </summary>
        public List<Modifier.FieldExpendituresModifier> ExpendituresModifiers { get; }


        //Constructor
        /// <summary>
        /// Constructs a Field from its coordinates, its Terrain and optionally from a height.
        /// </summary>
        /// <param name="_MapX">The X-Coordinate of the Field</param>
        /// <param name="_MapY">The Y-Coordinate of the Field</param>
        /// <param name="_FieldTerrainID">The TerrainID. For the constant IDs look at Terrain.terrainID_{Terrain}</param>
        /// <param name="_Height">The Height of this Field. Optional. Defaults to 0.</param>
        public Field(int _MapX, int _MapY, int _FieldTerrainID, int _Height = 0)
        {
            FieldID = CreateUniqueFieldID();
            Coordinates = new int[2] { _MapX, _MapY };
            FieldTerrain = new Terrain(_FieldTerrainID);
            Height = _Height;
            ParentEmpire = null;
            ParentCity = null;

            //Populate Lists
            FieldResources = new List<int>();
            FillFieldResourcesFromTerrainResources();

            MovementRateModifiers = new List<Modifier.FieldMovementRateModifier>();
            VisibilityModifiers = new List<Modifier.FieldVisibilityModifier>();
            ExpendituresModifiers = new List<Modifier.FieldExpendituresModifier>();
            FillFieldModifiersFromTerrainModifiers();

        }


        //Functions

        /// <summary>
        /// WIP. Creates a unique ID for the Field.
        /// </summary>
        /// <returns></returns>
        public int CreateUniqueFieldID()
        {
            //TO ADD: Everything.
            return 0;
        }
        /// <summary>
        /// Fills the List of Resources on the Field from the List of Resources on the Terrain.
        /// </summary>
        public void FillFieldResourcesFromTerrainResources()
        {
            foreach(int resource in FieldTerrain.TerrainResources)
            {
                FieldResources.Add(resource);
            }
        }

        /// <summary>
        /// Adds the modifiers for MovementRate, Expenditures and Visibility from the Terrain properties to the modifier Lists.
        /// </summary>
        public void FillFieldModifiersFromTerrainModifiers()
        {
            AddMovementRateModifier(Modifier.FieldMovementRateModifier.modifier_Terrain);
            AddExpendituresModifier(Modifier.FieldExpendituresModifier.modifier_Terrain);
            AddVisibilityModifier(Modifier.FieldVisibilityModifier.modifier_Terrain);
        }


        /// <summary>
        /// Adds the FieldMovementRateModifier to the MovementRateModifiers List. Replaces another modifier with the same ID. Sorts the new Entry: First alls additive, then all multiplicative modifiers. Calculates the new MovementRate value for the Field.
        /// </summary>
        /// <param name="_MovementRateModifier">The modifier to add to the list.</param>
        public void AddMovementRateModifier(Modifier.FieldMovementRateModifier _MovementRateModifier)
        {
            bool ModIsInserted = false;
            double movementRate = 1;
            for(int i = 0; i < MovementRateModifiers.Count; i++)
            {
                //Is there a modifier with the same ID? Replace it.
                if(_MovementRateModifier.ModifierID == MovementRateModifiers[i].ModifierID)
                {
                    MovementRateModifiers.RemoveAt(i);
                    MovementRateModifiers.Insert(i, _MovementRateModifier);
                    ModIsInserted = true;
                }
                //Insert the additive modifier at the end of all the additive modifiers, if there was no duplicate.
                if(!ModIsInserted && _MovementRateModifier.Additive && !MovementRateModifiers[i].Additive)
                {
                    MovementRateModifiers.Insert(i, _MovementRateModifier);
                    ModIsInserted = true;
                }
                //Insert the modifier, when the end of the list is reached and it wasn't inserted already.
                if(!ModIsInserted && i == MovementRateModifiers.Count-1)
                {
                    MovementRateModifiers.Add(_MovementRateModifier);
                    ModIsInserted = true;
                }

                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if(MovementRateModifiers[i].Additive)
                {
                    movementRate += MovementRateModifiers[i].Value;
                }
                else
                {
                    movementRate *= MovementRateModifiers[i].Value;
                }
            }

            MovementRate = movementRate;
        }
        /// <summary>
        /// Adds a MovementRateModifier to the MovementModifiers List by ID.
        /// </summary>
        /// <param name="_ModifierID"></param>
        /// <param name="_Value"></param>
        public void AddMovementRateModifier(int _ModifierID, double _Value = 1)
        {
            Modifier.FieldMovementRateModifier movementRateModifier = new Modifier.FieldMovementRateModifier(this, _ModifierID, _Value);
            AddMovementRateModifier(movementRateModifier);
        }
        /// <summary>
        /// Re-calculates the MovementRate from its modifiers List 
        /// </summary>
        public void UpdateMovementRate()
        {
            double movementRate = 1;
            for (int i = 0; i < MovementRateModifiers.Count; i++)
            {
                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if (MovementRateModifiers[i].Additive)
                {
                    movementRate += MovementRateModifiers[i].Value;
                }
                else
                {
                    movementRate *= MovementRateModifiers[i].Value;
                }
            }

            MovementRate = movementRate;
        }


        /// <summary>
        /// Adds the FieldMovementRateModifier to the MovementRateModifiers List. Replaces another modifier with the same ID. Sorts the new Entry: First alls additive, then all multiplicative modifiers. Calculates the new Expenditures value for the Field.
        /// </summary>
        /// <param name="_ExpendituresModifier">The modifier to add to the list.</param>
        public void AddExpendituresModifier(Modifier.FieldExpendituresModifier _ExpendituresModifier)
        {
            bool ModIsInserted = false;
            double expenditures = 1;
            for (int i = 0; i < ExpendituresModifiers.Count; i++)
            {
                //Is there a modifier with the same ID? Replace it.
                if (_ExpendituresModifier.ModifierID == ExpendituresModifiers[i].ModifierID)
                {
                    ExpendituresModifiers.RemoveAt(i);
                    ExpendituresModifiers.Insert(i, _ExpendituresModifier);
                    ModIsInserted = true;
                }
                //Insert the additive modifier at the end of all the additive modifiers, if there was no duplicate.
                if (!ModIsInserted && _ExpendituresModifier.Additive && !ExpendituresModifiers[i].Additive)
                {
                    ExpendituresModifiers.Insert(i, _ExpendituresModifier);
                    ModIsInserted = true;
                }
                //Insert the modifier, when the end of the list is reached and it wasn't inserted already.
                if (!ModIsInserted && i == ExpendituresModifiers.Count - 1)
                {
                    ExpendituresModifiers.Add(_ExpendituresModifier);
                    ModIsInserted = true;
                }

                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if (ExpendituresModifiers[i].Additive)
                {
                    expenditures += ExpendituresModifiers[i].Value;
                }
                else
                {
                    expenditures *= ExpendituresModifiers[i].Value;
                }
            }

            Expenditures = expenditures;
        }
        /// <summary>
        /// Adds a FieldExpendituresModifier to the ExpendituresModifiers List by ID.
        /// </summary>
        /// <param name="_ModifierID"></param>
        /// <param name="_Value"></param>
        public void AddExpendituresModifier(int _ModifierID, double _Value = 1)
        {
            Modifier.FieldExpendituresModifier expendituresModifier = new Modifier.FieldExpendituresModifier(this, _ModifierID, _Value);
            AddExpendituresModifier(expendituresModifier);
        }
        /// <summary>
        /// Re-calculates the Expenditures from its modifiers List
        /// </summary>
        public void UpdateExpenditures()
        {
            double expenditures = 1;

            for (int i = 0; i < ExpendituresModifiers.Count; i++)
            {
                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if (ExpendituresModifiers[i].Additive)
                {
                    expenditures += ExpendituresModifiers[i].Value;
                }
                else
                {
                    expenditures *= ExpendituresModifiers[i].Value;
                }
            }

            Expenditures = expenditures;
        }


        /// <summary>
        /// Adds the FieldVisibilityModifier to the VisibilityModifiers List. Replaces another modifier with the same ID. Sorts the new Entry: First alls additive, then all multiplicative modifiers. Calculates the new Visibility value for the Field.
        /// </summary>
        /// <param name="_VisibilityModifier">The modifier to add to the list.</param>
        public void AddVisibilityModifier(Modifier.FieldVisibilityModifier _VisibilityModifier)
        {
            bool ModIsInserted = false;
            double visibility = 1;
            for (int i = 0; i < VisibilityModifiers.Count; i++)
            {
                //Is there a modifier with the same ID? Replace it.
                if (_VisibilityModifier.ModifierID == VisibilityModifiers[i].ModifierID)
                {
                    VisibilityModifiers.RemoveAt(i);
                    VisibilityModifiers.Insert(i, _VisibilityModifier);
                    ModIsInserted = true;
                }
                //Insert the additive modifier at the end of all the additive modifiers, if there was no duplicate.
                if (!ModIsInserted && _VisibilityModifier.Additive && !VisibilityModifiers[i].Additive)
                {
                    VisibilityModifiers.Insert(i, _VisibilityModifier);
                    ModIsInserted = true;
                }
                //Insert the modifier, when the end of the list is reached and it wasn't inserted already.
                if (!ModIsInserted && i == VisibilityModifiers.Count - 1)
                {
                    VisibilityModifiers.Add(_VisibilityModifier);
                    ModIsInserted = true;
                }

                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if (VisibilityModifiers[i].Additive)
                {
                    visibility += VisibilityModifiers[i].Value;
                }
                else
                {
                    visibility *= VisibilityModifiers[i].Value;
                }
            }

            Visibility = visibility;
        }
        /// <summary>
        /// Adds a VisibilityModifier to the VisibilityModifiers List by ID.
        /// </summary>
        /// <param name="_ModifierID"></param>
        /// <param name="_Value"></param>
        public void AddVisibilityModifier(int _ModifierID, double _Value = 1)
        {
            Modifier.FieldVisibilityModifier visibilityModifier = new Modifier.FieldVisibilityModifier(this, _ModifierID, _Value);
            AddVisibilityModifier(visibilityModifier);
        }
        /// <summary>
        /// Re-calculates the Visibility from its modifiers List
        /// </summary>
        public void UpdateVisibility()
        {
            double visibility = 1;
            for (int i = 0; i < VisibilityModifiers.Count; i++)
            {
                //If the modifier at position i is Additive, add it. If it is a multiplicator, multiply it.
                if (VisibilityModifiers[i].Additive)
                {
                    visibility += VisibilityModifiers[i].Value;
                }
                else
                {
                    visibility *= VisibilityModifiers[i].Value;
                }
            }

            Visibility = visibility;
        }
    }
}
