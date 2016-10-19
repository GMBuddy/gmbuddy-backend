using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND5
{
    public class Sheet : ICharacterAttributes
    {
        public Guid SheetId { get; set; }
        public Guid CampaignCharacterId { get; set; }
        public CampaignCharacter CampaignCharacter { get; set; }

        public ICollection<Item> Items { get; set; }

        public int CharacterID { get; set; }

        // Implement character attributes

        //Key Character Attributes
        //Should we restrict class, race, level, race, size, gender, allignment to expected values?
        public string CharacterName { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public string Race { get; set; } 
        public string Allignment { get; set; }
        public int Experience { get; set; }
        public bool HasInspiration { get; set; }
        public int ProficiencyBonus { get; set; }
        public int AC { get; set; } //Calculated as 10 + ModifiersFromActivatedItems(Armor + Shield) + DexterityModifier [(DexterityValue - 10)/2] + SizeModifier (http://dungeons.wikia.com/wiki/Table:_Creature_Size_and_Scale_(3.5e_Other)) + NaturalArmor + DeflectionModifier
        public int MaxHitpoints { get; set; }
        public int CurrentHitpoints { get; set; }
        public string Age { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string EyeDescription { get; set; }
        public string HairDescription { get; set; }
        public string SkinDescription { get; set; }
        public string Diety { get; set; }
        public List<string> Organizations { get; set; }
        public string CharacterDescription { get; set; }
        public string Backstory { get; set; } //Redundant becauuse Bio is in Character.cs?
        public string AdditionalTraits { get; set; }

        //Ability Values (Not Modifiers!)
        public int Strength { get; set; }
        public int Dexternity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        //Saving Throws
        public int StrengthSave { get; set; }
        public int DexteritySave { get; set; }
        public int ConstitutionSave { get; set; }
        public int IntelligenceSave { get; set; }
        public int WisdomSave { get; set; }
        public int CharismaSave { get; set; }

        //Skill Ranks
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

        //Skill Proficiencies
        public bool AcrobaticsIsProf { get; set; } //Dexterity
        public bool AnimalHandlingIsProf { get; set; } //Wisdom
        public bool ArcanaIsProf { get; set; } //Intelligence
        public bool AthleticsIsProf  { get; set; } //Strength
        public bool DeceptionIsProf { get; set; } //Charisma
        public bool HistoryIsProf { get; set; } //Intelligence
        public bool InsightIsProf { get; set; } //Wisdom
        public bool IntimidationIsProf { get; set; } //Charisma
        public bool InvestigationIsProf { get; set; } //Intelligence
        public bool MedicineIsProf { get; set; } //Wisdom
        public bool NatureIsProf { get; set; } //Intelligence
        public bool PerceptionIsProf { get; set; } //Wisdom
        public bool PerformanceIsProf { get; set; } //Charisma
        public bool PersuationIsProf { get; set; } //Charisma
        public bool ReligionIsProf { get; set; } //Intelligence
        public bool SleightOfHandIsProf { get; set; } //Dexterity
        public bool StealthIsProf { get; set; } //Dexterity
        public bool SurvivalIsProf { get; set; } //Wisdom

                
        //Spell Stuff
        public string SpellCastingAbility { get; set; } //must be either Intelligence (INT), Wisdom (WIS), or Charisma (CHA)
        public int SpellSaveDC { get; set; }
        public int SpellAttackBonus { get; set; } //calculated by 10 + SpellCastingAbility Modifier + ProficiencyBonus
        public List<Spell> Spells { get; set; } //Need to make spell its own type so we can give it a description, spell save DC, spell level
    }
}
