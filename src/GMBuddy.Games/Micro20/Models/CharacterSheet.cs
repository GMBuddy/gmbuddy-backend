using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Micro20.Models
{
    public class CharacterSheet
    {
        private readonly Character character;

        public CharacterSheet(Character character)
        {
            this.character = character;
        }

        /// <summary>
        /// The character's STR attribute, including all buffs
        /// </summary>
        public int Strength
        {
            get
            {
                int strength = character.BaseStrength;

                if (character.Race == Micro20RaceType.Dwarf)
                {
                    strength += 2;
                }
                else if (character.Race == Micro20RaceType.Human)
                {
                    strength += 1;
                }

                return strength;
            }
        }

        /// <summary>
        /// The character's DEX attribute, including all buffs
        /// </summary>
        public int Dexterity
        {
            get
            {
                int dexterity = character.BaseDexterity;

                if (character.Race == Micro20RaceType.Halfling)
                {
                    dexterity += 2;
                }
                else if (character.Race == Micro20RaceType.Human)
                {
                    dexterity += 1;
                }

                return dexterity;
            }
        }

        /// <summary>
        /// The character's MIND attribute, including all buffs
        /// </summary>
        public int Mind
        {
            get
            {
                int mind = character.BaseMind;

                if (character.Race == Micro20RaceType.Elf)
                {
                    mind += 2;
                }
                else if (character.Race == Micro20RaceType.Human)
                {
                    mind += 1;
                }

                return mind;
            }
        }
    }
}
