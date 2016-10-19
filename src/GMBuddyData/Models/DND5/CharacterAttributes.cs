using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND5
{
    //abstract class CharacterAttributes
    //{
    //    Guid CharacterAttributesId { get; set; }
    //    int Stength { get; set; }
    //    int Charisma { get; set; }
    //    int Endurance { get; set; }

    //    //Guid CampaignCharacterId { get; set; }
    //    //CampaignCharacter CampaignCharacter { get; set; }
    //}

    interface ICharacterAttributes
    {
        
        int CharacterID { get; set; }

        //Ability Values (Not Modifiers!)
        int Strength { get; set; }
        int Dexternity { get; set; }
        int Constitution { get; set; }
        int Intelligence { get; set; }
        int Wisdom { get; set; }
        int Charisma { get; set; }

        //Skill Values
        //Calculate Value on FrontEnd as Ability Modifier [(AbilityValue - 10)/2] + ProficiencyModifier(If Proficient) + Ranks + Misc. Modifiers from Activated Items
        int Acrobatics { get; set; } //Dexterity
        int AnimalHandling { get; set; } //Wisdom
        int Arcana { get; set; } //Intelligence
        int Athletics  { get; set; } //Strength
        int Deception { get; set; } //Charisma
        int History { get; set; } //Intelligence
        int Insight { get; set; } //Wisdom
        int Intimidation { get; set; } //Charisma
        int Investigation { get; set; } //Intelligence
        int Medicine { get; set; } //Wisdom
        int Nature { get; set; } //Intelligence
        int Perception { get; set; } //Wisdom
        int Performance { get; set; } //Charisma
        int Persuation { get; set; } //Charisma
        int Religion { get; set; } //Intelligence
        int SleightOfHand { get; set; } //Dexterity
        int Stealth { get; set; } //Dexterity
        int Survival { get; set; } //Wisdom
    }
}
