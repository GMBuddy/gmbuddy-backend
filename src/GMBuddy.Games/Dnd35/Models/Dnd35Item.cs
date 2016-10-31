using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Dnd35Item
    {
        //how do I set attributes as key/required?
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string CharacterId { get; set; }

        public bool IsActive { get; set; }

        //Modifiers
        public int AC { get; set; }
        public int TouchAC { get; set; }
        public int FlatFootedAC { get; set; }
        public int MaxHitpoints { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int FortitudeSave { get; set; } //Constitution
        public int ReflexSave { get; set; } //Dexterity
        public int WillSave { get; set; } //Wisdom

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
    }
}
