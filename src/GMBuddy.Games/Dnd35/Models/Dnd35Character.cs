using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Dnd35Character
    {
        public Guid CharacterId { get; set; }
        public string UserId { get; set; }
        public string CampaignId { get; set; }
        public string Name { get; set; }

        //Miscellaneous attributes
        //TODO: restrict class, race, level, size, gender, allignment to expected values using enums
        public string Class { get; set; }
        public int Level { get; set; }
        public string Race { get; set; }
        public string Size { get; set; }
        public string Gender { get; set; }
        public string Allignment { get; set; }
        public string Diety { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Looks { get; set; }
        //TODO: Add regex validation for languages, feats, racialTraitsAndFeatures, and spells so they can be parsed and queried later
        public string Languages { get; set; }
        public string Feats { get; set; }
        public string RacialTraitsAndClassFeatures { get; set; }
        public string Spells { get; set; } //TODO: Make spell its own Class so we can give it a description, spell save DC, spell level
        public int SpellSaveDC { get; set; }
        public int CarryCapacityLight { get; set; }
        public int CarryCapacityMedium { get; set; }
        public int CarryCapacityHeavy { get; set; }
        public int Experience { get; set; }
        public int AC { get; set; } //Calculated as 10 + ModifiersFromActivatedItems(Armor + Shield) + DexterityModifier [(DexterityValue - 10)/2] + SizeModifier (http://dungeons.wikia.com/wiki/Table:_Creature_Size_and_Scale_(3.5e_Other)) + NaturalArmor + DeflectionModifier
        public int TouchAC { get; set; }
        public int FlatFootedAC { get; set; }
        public int MaxHitpoints { get; set; }
        public int CurrentHitpoints { get; set; }

        //Ability Values (Not Modifiers!)
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        //Saving Throws
        public int FortitudeSave { get; set; } //Constitution
        public int ReflexSave { get; set; } //Dexterity
        public int WillSave { get; set; } //Wisdom

        //Skill Ranks
        //Calculate Value on FrontEnd as Ability Modifier [(AbilityValue - 10)/2] + ProficiencyModifier(If Proficient) + Ranks + Misc. Modifiers from Activated Items
        public int Appraise { get; set; } //Intelligence
        public int Balance { get; set; } //Dexterity
        public int Bluff { get; set; } //Charisma
        public int Climb { get; set; } //Stength
        public int Concentration { get; set; } //Constitution
        public int Craft1 { get; set; } //Intelligence
        public string Craft1Type { get; set; }
        public int Craft2 { get; set; } //Intelligence
        public string Craft2Type { get; set; }
        public int Craft3 { get; set; } //Intelligence
        public string Craft3Type { get; set; }
        public int DecipherScript { get; set; } //Intelligence
        public int Diplomacy { get; set; } //Charisma
        public int DisableDevice { get; set; } //Intelligence
        public int Disguise { get; set; } //Charisma
        public int EscapeArtist { get; set; } //Dexterity
        public int Forgery { get; set; } //Intelligence
        public int GatherInformation { get; set; } //Charisma
        public int HandleAnimal { get; set; } //Charisma
        public int Heal { get; set; } //Wisdom
        public int Hide { get; set; } //Dexterity
        public int Intimidate { get; set; } //Charisma
        public int Jump { get; set; } //Strength
        public int KnowledgeArcana { get; set; } //Intelligence
        public int KnowledgeArchitecture { get; set; } //Intelligence
        public int KnowledgeDungeoneering { get; set; } //Intelligence
        public int KnowledgeHistory { get; set; } //Intelligence
        public int KnowledgeLocal { get; set; } //Intelligence
        public int KnowledgeNature { get; set; } //Intelligence
        public int KnowledgeNobility { get; set; } //Intelligence
        public int KnowledgeThePlanes { get; set; } //Intelligence
        public int KnowledgeReligion { get; set; } //Intelligence
        public int KnowledgeOther { get; set; } //Intelligence
        public string KnowledgeOtherType { get; set; }
        public int Listen { get; set; } //Wisdom
        public int MoveSilently { get; set; } //Dexterity
        public int OpenLock { get; set; } //Dexterity
        public int PerformAct { get; set; } //Charisma
        public int PerformComedy { get; set; } //Charisma
        public int PerformDance { get; set; } //Charisma
        public int PerformKeyboard { get; set; } //Charisma
        public int PerformOratory { get; set; } //Charisma
        public int PerformPercussion { get; set; } //Charisma
        public int PerformString { get; set; } //Charisma
        public int PerformWind { get; set; } //Charisma
        public int PerformSing { get; set; } //Charisma
        public int PerformOther { get; set; } //Charisma
        public string PerformOtherType { get; set; }
        public int Profession1 { get; set; } //Wisdom
        public string Profession1Type { get; set; }
        public int Profession2 { get; set; } //Wisdom
        public int Profession2Type { get; set; }
        public int Ride { get; set; } //Dexterity
        public int Search { get; set; } //Intelligence
        public int SenseMotive { get; set; } //Wisdom
        public int SleightOfHand { get; set; } //Dexterity
        public int Spellcraft { get; set; } //Intelligence
        public int Spot { get; set; } //Wisdom
        public int Survival { get; set; } //Wisdom
        public int Swim { get; set; } //Strength
        public int Tumble { get; set; } //Dexterity
        public int UseMagicDevice { get; set; } //Charisma
        public int UseRope { get; set; } //Dexterity

        //Skill Proficiencies
        public bool AppraiseIsProf { get; set; }
        public bool BalanceIsProf { get; set; }
        public bool BluffIsProf { get; set; }
        public bool ClimbIsProf { get; set; }
        public bool ConcentrationIsProf { get; set; }
        public bool Craft1IsProf { get; set; }
        public bool Craft2IsProf { get; set; }
        public bool Craft3IsProf { get; set; }
        public bool DecipherScriptIsProf { get; set; }
        public bool DiplomacyIsProf { get; set; }
        public bool DisableDeviceIsProf { get; set; }
        public bool DisguiseIsProf { get; set; }
        public bool EscapeArtistIsProf { get; set; }
        public bool ForgeryIsProf { get; set; }
        public bool GatherInformationIsProf { get; set; }
        public bool HandleAnimalIsProf { get; set; }
        public bool HealIsProf { get; set; }
        public bool HideIsProf { get; set; }
        public bool IntimidateIsProf { get; set; }
        public bool JumpIsProf { get; set; }
        public bool KnowledgeArcanaIsProf { get; set; }
        public bool KnowledgeArchitectureIsProf { get; set; }
        public bool KnowledgeDungeoneeringIsProf { get; set; }
        public bool KnowledgeHistoryIsProf { get; set; }
        public bool KnowledgeLocalIsProf { get; set; }
        public bool KnowledgeNatureIsProf { get; set; }
        public bool KnowledgeNobilityIsProf { get; set; }
        public bool KnowledgeThePlanesIsProf { get; set; }
        public bool KnowledgeReligionIsProf { get; set; }
        public bool KnowledgeOtherIsProf { get; set; }
        public bool ListenIsProf { get; set; }
        public bool MoveSilentlyIsProf { get; set; }
        public bool OpenLockIsProf { get; set; }
        public bool PerformActIsProf { get; set; }
        public bool PerformComedyIsProf { get; set; }
        public bool PerformDanceIsProf { get; set; }
        public bool PerformKeyboardIsProf { get; set; }
        public bool PerformOratoryIsProf { get; set; }
        public bool PerformPercussionIsProf { get; set; }
        public bool PerformStringIsProf { get; set; }
        public bool PerformWindIsProf { get; set; }
        public bool PerformSingIsProf { get; set; }
        public bool PerformOtherIsProf { get; set; }
        public bool Profession1IsProf { get; set; }
        public bool Profession2IsProf { get; set; }
        public bool RideIsProf { get; set; }
        public bool SearchIsProf { get; set; }
        public bool SenseMotiveIsProf { get; set; }
        public bool SleightOfHandIsProf { get; set; }
        public bool SpellcraftIsProf { get; set; }
        public bool SpotIsProf { get; set; }
        public bool SurvivalIsProf { get; set; }
        public bool SwimIsProf { get; set; }
        public bool TumbleIsProf { get; set; }
        public bool UseMagicDeviceIsProf { get; set; }
        public bool UseRopeIsProf { get; set; }

    }
}
