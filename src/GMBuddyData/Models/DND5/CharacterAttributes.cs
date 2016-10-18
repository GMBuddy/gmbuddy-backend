using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
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
        //Miscellaneous attributes
        //Should we restrict class, race, level, race, size, gender, allignment to expected values?
        string CharacterName { get; set; }
        string Class { get; set; }
        int Level { get; set; }
        string Race { get; set; } 
        string Size { get; set; }
        string Gender { get; set; }
        string Allignment { get; set; }
        string Diety { get; set; }
        string Height { get; set; }
        string Weight { get; set; }
        string Looks { get; set; }
        List<string> Languages { get; set; }
        List<string> Feats { get; set; }
        List<string> RacialTraitsAndClassFeatures { get; set; }
        List<Spell> Spells { get; set; } //Need to make spell its own type so we can give it a description, spell save DC, spell level
        int SpellSaveDC { get; set; }
        int CarryCapacityLight { get; set; }
        int CarryCapacityMedium { get; set; }
        int CarryCapacityHeavy { get; set; }
        int Experience { get; set; }
        int AC { get; set; } //Calculated as 10 + ModifiersFromActivatedItems(Armor + Shield) + DexterityModifier [(DexterityValue - 10)/2] + SizeModifier (http://dungeons.wikia.com/wiki/Table:_Creature_Size_and_Scale_(3.5e_Other)) + NaturalArmor + DeflectionModifier
        int TouchAC { get; set; }
        int FlatFootedAC { get; set; }
        int MaxHitpoints { get; set; }
        int CurrentHitpoints { get; set; }

        //Ability Values (Not Modifiers!)
        int Strength { get; set; }
        int Dexternity { get; set; }
        int Constitution { get; set; }
        int Intelligence { get; set; }
        int Wisdom { get; set; }
        int Charisma { get; set; }

        //Saving Throws
        int FortitudeSave { get; set; } //Constitution
        int ReflexSave { get; set; } //Dexterity
        int WillSave { get; set; } //Wisdom

        //Skill Ranks
        //Calculate Value on FrontEnd as Ability Modifier [(AbilityValue - 10)/2] + ProficiencyModifier(If Proficient) + Ranks + Misc. Modifiers from Activated Items
        int Appraise { get; set; } //Intelligence
        int Balance { get; set; } //Dexterity
        int Bluff { get; set; } //Charisma
        int Climb { get; set; } //Stength
        int Concentration { get; set; } //Constitution
        int Craft1 { get; set; } //Intelligence
        string Craft1Type { get; set; }
        int Craft2 { get; set; } //Intelligence
        string Craft2Type { get; set; }
        int Craft3 { get; set; } //Intelligence
        string Craft3Type { get; set; }
        int DecipherScript { get; set; } //Intelligence
        int Diplomacy { get; set; } //Charisma
        int DisableDevice { get; set; } //Intelligence
        int Disguise { get; set; } //Charisma
        int EscapeArtist { get; set; } //Dexterity
        int Forgery { get; set; } //Intelligence
        int GatherInformation { get; set; } //Charisma
        int HandleAnimal { get; set; } //Charisma
        int Heal { get; set; } //Wisdom
        int Hide { get; set; } //Dexterity
        int Intimidate { get; set; } //Charisma
        int Jump { get; set; } //Strength
        int KnowledgeArcana { get; set; } //Intelligence
        int KnowledgeArchitecture { get; set; } //Intelligence
        int KnowledgeDungeoneering { get; set; } //Intelligence
        int KnowledgeHistory { get; set; } //Intelligence
        int KnowledgeLocal { get; set; } //Intelligence
        int KnowledgeNature { get; set; } //Intelligence
        int KnowledgeNobility { get; set; } //Intelligence
        int KnowledgeThePlanes { get; set; } //Intelligence
        int KnowledgeReligion { get; set; } //Intelligence
        int KnowledgeOther { get; set; } //Intelligence
        int Listen { get; set; } //Wisdom
        int MoveSilently { get; set; } //Dexterity
        int OpenLock { get; set; } //Dexterity
        int PerformAct { get; set; } //Charisma
        int PerformComedy { get; set; } //Charisma
        int PerformDance { get; set; } //Charisma
        int PerformKeyboard { get; set; } //Charisma
        int PerformOratory { get; set; } //Charisma
        int PerformPercussion { get; set; } //Charisma
        int PerformString { get; set; } //Charisma
        int PerformWind { get; set; } //Charisma
        int PerformSing { get; set; } //Charisma
        int PerformOther { get; set; } //Charisma
        string PerformOtherType { get; set; }
        int Profession1 { get; set; } //Wisdom
        string Profession1Type { get; set; }
        int Profession2 { get; set; } //Wisdom
        int Profession2Type { get; set; }
        int Ride { get; set; } //Dexterity
        int Search { get; set; } //Intelligence
        int SenseMotive { get; set; } //Wisdom
        int SleightOfHand { get; set; } //Dexterity
        int Spellcraft { get; set; } //Intelligence
        int Spot { get; set; } //Wisdom
        int Survival { get; set; } //Wisdom
        int Swim { get; set; } //Strength
        int Tumble { get; set; } //Dexterity
        int UseMagicDevice { get; set; } //Charisma
        int UseRope { get; set; } //Dexterity

        //Skill Proficiencies
        bool AppraiseIsProf { get; set; } 
        bool BalanceIsProf { get; set; } 
        bool BluffIsProf { get; set; }
        bool ClimbIsProf { get; set; }
        bool ConcentrationIsProf { get; set; }
        bool Craft1IsProf { get; set; }
        bool Craft2IsProf { get; set; }
        bool Craft3IsProf { get; set; }
        bool DecipherScriptIsProf { get; set; }
        bool DiplomacyIsProf { get; set; }
        bool DisableDeviceIsProf { get; set; }
        bool DisguiseIsProf { get; set; }
        bool EscapeArtistIsProf { get; set; }
        bool ForgeryIsProf { get; set; }
        bool GatherInformationIsProf { get; set; }
        bool HandleAnimalIsProf { get; set; }
        bool HealIsProf { get; set; } 
        bool HideIsProf { get; set; }
        bool IntimidateIsProf { get; set; }
        bool JumpIsProf { get; set; }
        bool KnowledgeArcanaIsProf { get; set; }
        bool KnowledgeArchitectureIsProf { get; set; }
        bool KnowledgeDungeoneeringIsProf { get; set; }
        bool KnowledgeHistoryIsProf { get; set; }
        bool KnowledgeLocalIsProf { get; set; }
        bool KnowledgeNatureIsProf { get; set; }
        bool KnowledgeNobilityIsProf { get; set; }
        bool KnowledgeThePlanesIsProf { get; set; }
        bool KnowledgeReligionIsProf { get; set; }
        bool KnowledgeOtherIsProf { get; set; }
        bool ListenIsProf { get; set; } 
        bool MoveSilentlyIsProf { get; set; }
        bool OpenLockIsProf { get; set; }
        bool PerformActIsProf { get; set; }
        bool PerformComedyIsProf { get; set; }
        bool PerformDanceIsProf { get; set; }
        bool PerformKeyboardIsProf { get; set; }
        bool PerformOratoryIsProf { get; set; }
        bool PerformPercussionIsProf { get; set; }
        bool PerformStringIsProf { get; set; }
        bool PerformWindIsProf { get; set; }
        bool PerformSingIsProf { get; set; }
        bool PerformOtherIsProf { get; set; }
        bool Profession1IsProf { get; set; } 
        bool Profession2IsProf { get; set; } 
        bool RideIsProf { get; set; }
        bool SearchIsProf { get; set; }
        bool SenseMotiveIsProf { get; set; }
        bool SleightOfHandIsProf { get; set; }
        bool SpellcraftIsProf { get; set; }
        bool SpotIsProf { get; set; }
        bool SurvivalIsProf { get; set; }
        bool SwimIsProf { get; set; }
        bool TumbleIsProf { get; set; }
        bool UseMagicDeviceIsProf { get; set; }
        bool UseRopeIsProf { get; set; }
    }
}
