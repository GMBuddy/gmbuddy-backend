using System;
using GMBuddy.Games.Micro20.Models;
using System.Collections.Generic;

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

    public class Skills
    {
        private readonly Character c;

        public Skills(Character c)
        {
            this.c = c;
        }

        public int Physical => c.BasePhysical;

        public int Subterfuge => c.BaseSubterfuge;

        public int Knowledge => c.BaseKnowledge;

        public int Communication => c.BaseCommunication;
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

        public int Physcial
        {
            get
            {
                int rank = c.Level;
                if(c.Class == Micro20ClassType.Fighter)
                        rank += 3;
                if(c.Race == Micro20RaceType.Human)
                        rank += 2;
                return rank;
            }
        }

        public int Subterfuge
        {
            get
            {
                int rank = c.Level;
                if(c.Class == Micro20ClassType.Rogue)
                        rank += 3;
                if(c.Race == Micro20RaceType.Human)
                        rank += 2;
                return rank;
            }
        }

        public int Knowledge
        {
            get
            {
                int rank = c.Level;
                if(c.Class == Micro20ClassType.Magi)
                        rank += 3;
                if(c.Race == Micro20RaceType.Human)
                        rank += 2;
                return rank;
            }
        }

        public int Communication
        {
            get
            {
                int rank = c.Level;
                if(c.Class == Micro20ClassType.Cleric)
                        rank += 3;
                if(c.Race == Micro20RaceType.Human)
                        rank += 2;
                return rank;
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

        public Guid? CampaignId => c.CampaignId;

        public string UserId => c.UserId;

        public string Name => c.Name;

        public string Height => c.Height;

        public string Weight => c.Weight;

        public string HairColor => c.HairColor;

        public string EyeColor => c.EyeColor;

        public Micro20RaceType Race => c.Race;

        public Micro20ClassType Class => c.Class;

        public int Level => c.Level;

        public int Experience => c.Experience;

        internal ICollection<Item> Items => c.Items;

        internal ICollection<Spell> Spells => c.Spells;

        public int CopperPieces => c.CopperPieces;
        
        public int SilverPieces => c.SilverPieces;

        public int GoldPieces => c.GoldPieces;

        public int PlatinumPieces => c.PlatinumPieces;
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
        public Skills Skills => new Skills(c);
    }
}
