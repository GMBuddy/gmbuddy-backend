using System;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.OutputModels
{
    public class BaseStats
    {
        private readonly Character c;

        public BaseStats(Character c)
        {
            this.c = c;
        }

        public int Strength => c.BaseStrength;

        public int Dexterity => c.BaseDexterity;

        public int Mind => c.BaseMind;
    }

    /// <summary>
    /// Contains properties for all fields that can be modified in any capacity.
    /// </summary>
    /// <example>
    /// int ActualStrength = sheet.BaseStats.Strength + sheet.Modifiers.Strength
    /// </example>
    public class Modifiers
    {
        private readonly Character c;

        public Modifiers(Character c)
        {
            this.c = c;
        }

        public int Strength
        {
            get
            {
                switch (c.Race)
                {
                    case Micro20RaceType.Dwarf:
                        return 2;
                    case Micro20RaceType.Human:
                        return 1;
                    case Micro20RaceType.Elf:
                    case Micro20RaceType.Halfling:
                    default:
                        return 0;
                }
            }
        }

        public int Dexterity
        {
            get
            {
                switch (c.Race)
                {
                    case Micro20RaceType.Halfling:
                        return 2;
                    case Micro20RaceType.Human:
                        return 1;
                    case Micro20RaceType.Elf:
                    case Micro20RaceType.Dwarf:
                    default:
                        return 0;
                }
            }
        }

        public int Mind
        {
            get
            {
                switch (c.Race)
                {
                    case Micro20RaceType.Elf:
                        return 2;
                    case Micro20RaceType.Human:
                        return 1;
                    case Micro20RaceType.Dwarf:
                    case Micro20RaceType.Halfling:
                    default:
                        return 0;
                }
            }
        }
    }

    public class Details
    {
        private readonly Character c;

        public Details(Character c)
        {
            this.c = c;
        }

        public Guid CharacterId => c.CharacterId;

        public string UserId => c.UserId;

        public string Name => c.Name;

        public string Height => c.Height;

        public string Weight => c.Weight;

        public string HairColor => c.HairColor;

        public string EyeColor => c.EyeColor;

        public Micro20RaceType Race => c.Race;

        public Micro20ClassType Class => c.Class;

        public int Level => c.Level;
    }

    public class CharacterSheet
    {
        private readonly Character c;

        public CharacterSheet(Character c)
        {
            this.c = c;
        }

        public Details Details => new Details(c);

        public BaseStats BaseStats => new BaseStats(c);

        public Modifiers Modifiers => new Modifiers(c);
    }
}
