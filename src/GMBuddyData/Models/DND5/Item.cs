using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND5
{
    public class Item : ICharacterAttributes
    {
        public Guid ItemId { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public ItemType ItemType { get; set; }
        public string ItemDescription { get; set; }

        public Guid SheetId { get; set; }
        public Sheet Sheet { get; set; }
        
        public int CharacterID { get; set; }

        //Ability Values (Not Modifiers!)
        public int Strength { get; set; }
        public int Dexternity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        //Skill Values
        //Calculate Value on FrontEnd as Ability Modifier [(AbilityValue - 10)/2] + ProficiencyModifier(If Proficient) + Ranks + Misc. Modifiers from Activated Items
        public int Acrobatics { get; set; } //Dexterity
        public int AnimalHandling { get; set; } //Wisdom
        public int Arcana { get; set; } //Intelligence
        public int Athletics  { get; set; } //Strength
        public int Deception { get; set; } //Charisma
        public int History { get; set; } //Intelligence
        public int Insight { get; set; } //Wisdom
        public int Intimidation { get; set; } //Charisma
        public int Investigation { get; set; } //Intelligence
        public int Medicine { get; set; } //Wisdom
        public int Nature { get; set; } //Intelligence
        public int Perception { get; set; } //Wisdom
        public int Performance { get; set; } //Charisma
        public int Persuation { get; set; } //Charisma
        public int Religion { get; set; } //Intelligence
        public int SleightOfHand { get; set; } //Dexterity
        public int Stealth { get; set; } //Dexterity
        public int Survival { get; set; } //Wisdom
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Generic
    }
}
