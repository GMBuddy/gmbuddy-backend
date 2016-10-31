using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35.Data;
using GMBuddy.Games.Dnd35.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Dnd35
{
    public class Dnd35GameService
    {
        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Dnd35Campaign>> GetCampaignsAsync()
        {
            using (var db = new Dnd35DataContext())
            {
                return await db.Campaigns.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a campaign with the given campaign name and GM's userId to the database
        /// </summary>
        /// <param name="name">The name of the campaign</param>
        /// <param name="userId">The GM's userId address (uniquely identifies the GM)</param>
        /// <returns>The ID of the added campaign</returns>
        public async Task<Guid> AddCampaignAsync(string name, string userId)
        {
            using (var db = new Dnd35DataContext())
            {
                var campaign = new Dnd35Campaign
                {
                    Name = name,
                    GmUserId = userId
                };

                db.Campaigns.Add(campaign);

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save campaign");
                }

                return campaign.CampaignId;
            }
        }

        /// <summary>
        /// Get all characters (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of characters. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Dnd35Character>> GetCharactersAsync()
        {
            using (var db = new Dnd35DataContext())
            {
                return await db.Characters.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a character with the given characterName and bio to the database along with the user's email and userId
        /// </summary>
        /// <param name="userId">The email of the user creating the character</param>
        /// <param name="name">The name of the character to be created</param>
        /// <returns>The ID of the added character</returns>
        public async Task<Guid> AddCharacterAsync(string userId, string name)
        {
            using (var db = new Dnd35DataContext())
            {
                var character = new Dnd35Character
                {
                    UserId = userId,
                    Name = name
                };

                db.Characters.Add(character);

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save campaign");
                }

                return character.CharacterId;
            }
        }

        /// <summary>
        /// Adds a stat with the given name and value to a character in the database
        /// </summary>
        /// <param name="userId">The email of the user adding the stat to the character, should either be the user who made the character or the GM of that user</param>
        /// <param name="characterId">The characterId of the character to which the stat is being added</param>
        /// All the oher parameters are able to be null, and they are the values of the attributes to be modified
        /// <returns>The characterId of the modified character (because I didn't know what else to return)</returns>
        public async Task<Guid> ModifyCharacterAttributesAsync(string userId, string characterId, string characterClass,
        int? level, string race, string size, string gender, string allignment,
        string diety, string height, string weight, string looks, string languages, string feats,
        string racialTraitsAndFeatures, int? spellSaveDC, int? carryCapacityLight, int? carryCapacityMedium,
        int? carryCapacityHeavy, int? experience, int? normalAC, int? touchAC, int? flatFootedAC, int? maxHitpoints,
        int? currentHitpoints, int? strength, int? dexterity, int? constitution, int? intelligence, int? wisdom,
        int? charisma, int? fortitudeSave, int? reflexSave, int? willSave, int? appraise, int? balance, int? bluff,
        int? climb, int? concentration, int? craft1, string craft1Type, int? craft2, string craft2Type, int? craft3,
        string craft3Type, int? decipherScript, int? diplomacy, int? disableDevice, int? disguise, int? escapeArtist,
        int? forgery, int? gatherInformation, int? handleAnimal, int? heal, int? hide, int? intimidate, int? jump,
        int? knowledgeArcana, int? knowledgeArchitecture, int? knowledgeDungeoneering, int? knowledgeHistory,
        int? knowledgeLocal, int? knowledgeNature, int? knowledgeNobility, int? knowledgeThePlanes, int? knowledgeReligion,
        int? knowledgeOther, string knowledgeOtherType, int? listen, int? moveSilently, int? openLock, int? performAct,
        int? performComedy, int? performDance, int? performKeyboard, int? performOratory, int? performPercussion,
        int? performString, int? performWind, int? performSing, int? performOther, string performOtherType, int? profession1,
        string profession1Type, int? profession2, string profession2Type, int? ride, int? search, int? senseMotive,
        int? sleightOfHand, int? spellcraft, int? spot, int? survival, int? swim, int? tumble, int? useMagicDevice,
        int? useRope)
        {
            using (var db = new Dnd35DataContext())
            {
                //get the character, add the key value pair to the character's stat dictionary
                var characters = await db.Characters.ToListAsync();
                var character = characters.Single(c => (c.CharacterId.ToString().Equals(characterId)));
                /*
                if(character.UserId != userId)
                {
                    throw new Exception("Current user does not have access to modify this campaign"); //Should we even check for this? Should we also allow GM to modify a player's stats?
                }
                */
                character.Class = characterClass ?? character.Class;

                character.Level = level ?? character.Level;
                character.Race = race ?? character.Race;

                character.Size = size;

                character.Gender = gender ?? character.Gender;
                character.Allignment = allignment ?? character.Allignment;
                character.Diety = diety ?? character.Diety;
                character.Height = height ?? character.Height;
                character.Weight = weight ?? character.Weight;
                character.Looks = looks ?? character.Looks;
                character.Languages = languages ?? character.Languages;
                character.Feats = feats ?? character.Feats;
                character.RacialTraitsAndClassFeatures = racialTraitsAndFeatures ?? character.RacialTraitsAndClassFeatures;
                character.SpellSaveDC = spellSaveDC ?? character.SpellSaveDC;
                character.CarryCapacityLight = carryCapacityLight ?? character.CarryCapacityLight;
                character.CarryCapacityMedium = carryCapacityMedium ?? character.CarryCapacityMedium;
                character.CarryCapacityHeavy = carryCapacityHeavy ?? character.CarryCapacityHeavy;
                character.Experience = experience ?? character.Experience;
                character.AC = normalAC ?? character.AC;
                character.TouchAC = touchAC ?? character.TouchAC;
                character.FlatFootedAC = flatFootedAC ?? character.FlatFootedAC;
                character.MaxHitpoints = maxHitpoints ?? character.MaxHitpoints;
                character.CurrentHitpoints = currentHitpoints ?? character.CurrentHitpoints;
                character.Strength = strength ?? character.Strength;
                character.Dexterity = dexterity ?? character.Dexterity;
                character.Constitution = constitution ?? character.Constitution;
                character.Intelligence = intelligence ?? character.Intelligence;
                character.Wisdom = wisdom ?? character.Wisdom;
                character.Charisma = charisma ?? character.Charisma;
                character.FortitudeSave = fortitudeSave ?? character.FortitudeSave;
                character.ReflexSave = reflexSave ?? character.ReflexSave;
                character.WillSave = willSave ?? character.WillSave;
                character.Appraise = appraise ?? character.Appraise;
                character.Balance = balance ?? character.Balance;
                character.Bluff = bluff ?? character.Bluff;
                character.Climb = climb ?? character.Climb;
                character.Concentration = concentration ?? character.Concentration;
                character.Craft1 = craft1 ?? character.Craft1;
                character.Craft1Type = craft1Type ?? character.Craft1Type;
                character.Craft2 = craft2 ?? character.Craft2;
                character.Craft2Type = craft2Type ?? character.Craft2Type;
                character.DecipherScript = decipherScript ?? character.DecipherScript;
                character.Diplomacy = diplomacy ?? character.Diplomacy;
                character.DisableDevice = disableDevice ?? character.DisableDevice;
                character.Disguise = disguise ?? character.Disguise;
                character.EscapeArtist = escapeArtist ?? character.EscapeArtist;
                character.Forgery = forgery ?? character.Forgery;
                character.GatherInformation = gatherInformation ?? character.GatherInformation;
                character.HandleAnimal = handleAnimal ?? character.HandleAnimal;
                character.Heal = heal ?? character.Heal;
                character.Hide = hide ?? character.Hide;
                character.Intimidate = intimidate ?? character.Intimidate;
                character.Jump = jump ?? character.Jump;
                character.KnowledgeArcana = knowledgeArcana ?? character.KnowledgeArcana;
                character.KnowledgeArchitecture = knowledgeArchitecture ?? character.KnowledgeArchitecture;
                character.KnowledgeDungeoneering = knowledgeDungeoneering ?? character.KnowledgeDungeoneering;
                character.KnowledgeHistory = knowledgeHistory ?? character.KnowledgeHistory;
                character.KnowledgeDungeoneering = knowledgeDungeoneering ?? character.KnowledgeDungeoneering;
                character.KnowledgeHistory = knowledgeHistory ?? character.KnowledgeHistory;
                character.KnowledgeLocal = knowledgeLocal ?? character.KnowledgeLocal;
                character.KnowledgeNature = knowledgeNature ?? character.KnowledgeNature;
                character.KnowledgeNobility = knowledgeNobility ?? character.KnowledgeNobility;
                character.KnowledgeThePlanes = knowledgeThePlanes ?? character.KnowledgeThePlanes;
                character.KnowledgeReligion = knowledgeReligion ?? character.KnowledgeReligion;
                character.KnowledgeOther = knowledgeOther ?? character.KnowledgeOther;
                character.KnowledgeOtherType = knowledgeOtherType ?? character.KnowledgeOtherType;
                character.Listen = listen ?? character.Listen;
                character.MoveSilently = moveSilently ?? character.MoveSilently;
                character.OpenLock = openLock ?? character.OpenLock;
                character.PerformAct = performAct ?? character.PerformAct;
                character.PerformComedy = performComedy ?? character.PerformComedy;
                character.PerformDance = performDance ?? character.PerformDance;
                character.PerformKeyboard = performKeyboard ?? character.PerformKeyboard;
                character.PerformOratory = performOratory ?? character.PerformOratory;
                character.PerformPercussion = performPercussion ?? character.PerformPercussion;
                character.PerformString = performString ?? character.PerformString;
                character.PerformWind = performWind ?? character.PerformWind;
                character.PerformSing = performSing ?? character.PerformSing;
                character.PerformOther = performOther ?? character.PerformOther;
                character.PerformOtherType = performOtherType ?? character.PerformOtherType;
                character.Profession1 = profession1 ?? character.Profession1;
                character.Profession1Type = profession1Type ?? character.Profession1Type;
                character.Profession2 = profession2 ?? character.Profession2;
                character.Profession2Type = profession2Type ?? character.Profession2Type;
                character.Ride = ride ?? character.Ride;
                character.Search = search ?? character.Search;
                character.SenseMotive = senseMotive ?? character.SenseMotive;
                character.SleightOfHand = sleightOfHand ?? character.SleightOfHand;
                character.Spellcraft = spellcraft ?? character.Spellcraft;
                character.Spot = spot ?? character.Spot;
                character.Survival = survival ?? character.Survival;
                character.Swim = swim ?? character.Swim;
                character.Tumble = tumble ?? character.Tumble;
                character.UseMagicDevice = useMagicDevice ?? character.UseMagicDevice;
                character.UseRope = useRope ?? character.UseRope;

                //TODO: Implement Proficiency Booleans.

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save campaign");
                }

                return character.CharacterId;
            }
        }

        /// <summary>
        /// Adds a character with the given characterId and value to a campaign in the database
        /// </summary>
        /// <param name="userId">The email of the user assigning the character to the campaign</param>
        /// <param name="characterId">The characterId of the character to be added to the campaign</param>
        /// <param name="campaignId">The campaignId of the campaign to which the character is being added</param>
        /// <returns>The campaignId of the modified campaign (because I didn't know what else to return)</returns>
        public async Task<string> AssignCharacterToCampaignAsync(string userId, string characterId, string campaignId)
        {
            //TODO: redesign this using the concept of CampaignCharacter so that seperate versions of a character can be added to different campaigns (ugh)
            using (var db = new Dnd35DataContext())
            {
                var characters = await db.Characters.ToListAsync();
                var character = characters.Single(c => c.CharacterId.ToString().Equals(characterId));

                if(character.UserId != userId)
                {
                    throw new Exception("Current user does not have access to modify this character"); //Should we even check for this? Should we also allow GM to modify a player's stats?
                }

                character.CampaignId = campaignId;

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not update character");
                }

                return character.CampaignId;
            }
        }

        /// <summary>
        /// Get all items (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of items. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Dnd35Item>> GetItemsAsync()
        {
            using (var db = new Dnd35DataContext())
            {
                return await db.Items.ToListAsync();
            }
        }

        /// <summary>
        /// Adds an item with the given name and description to the database
        /// </summary>
        /// <param name="name">The name of the item to be created</param>
        /// <param name="description">The description of the item to be created</param>
        /// <returns>The ID of the added item</returns>
        public async Task<Guid> AddItemAsync(string name, string description)
        {
            using (var db = new Dnd35DataContext())
            {
                var item = new Dnd35Item
                {
                    ItemName = name,
                    ItemDescription = description
                };

                db.Items.Add(item);

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save item");
                }

                return item.ItemId;
            }
        }

        /// <summary>
        /// Updates the modifers of an item with the given values in the database
        /// </summary>
        /// <param name="userId">The email of the user adding the modifier to the item, should either be the user who currently owns the item or the GM of that user</param>
        /// <param name="itemId">The itemId of the item to which the modifier is being added</param>
        /// All the oher parameters are able to be null, and they are the values of the attributes to be modified
        /// <returns>The characterId of the modified character (because I didn't know what else to return)</returns>
        public async Task<Guid> ModifyItemAttributesAsync(string itemId, int? normalAC, int? touchAC, int? flatFootedAC, int? maxHitpoints,
        int? strength, int? dexterity, int? constitution, int? intelligence, int? wisdom,
        int? charisma, int? fortitudeSave, int? reflexSave, int? willSave, int? appraise, int? balance, int? bluff,
        int? climb, int? concentration, int? craft1, int? craft2, int? craft3,
        int? decipherScript, int? diplomacy, int? disableDevice, int? disguise, int? escapeArtist,
        int? forgery, int? gatherInformation, int? handleAnimal, int? heal, int? hide, int? intimidate, int? jump,
        int? knowledgeArcana, int? knowledgeArchitecture, int? knowledgeDungeoneering, int? knowledgeHistory,
        int? knowledgeLocal, int? knowledgeNature, int? knowledgeNobility, int? knowledgeThePlanes, int? knowledgeReligion,
        int? knowledgeOther, int? listen, int? moveSilently, int? openLock, int? performAct,
        int? performComedy, int? performDance, int? performKeyboard, int? performOratory, int? performPercussion,
        int? performString, int? performWind, int? performSing, int? performOther, int? profession1,
        int? profession2, int? ride, int? search, int? senseMotive, int? sleightOfHand, int? spellcraft, int? spot, int? survival, int? swim, int? tumble, int? useMagicDevice, int? useRope)
        {
            using (var db = new Dnd35DataContext())
            {
                //get the item
                var items = await db.Items.ToListAsync();
                var item = items.Single(i => (i.ItemId.ToString().Equals(itemId)));
                //modify any attributes that are not null
                item.AC = normalAC ?? item.AC;
                item.TouchAC = touchAC ?? item.TouchAC;
                item.FlatFootedAC = flatFootedAC ?? item.FlatFootedAC;
                item.MaxHitpoints = maxHitpoints ?? item.MaxHitpoints;
                item.Strength = strength ?? item.Strength;
                item.Dexterity = dexterity ?? item.Dexterity;
                item.Constitution = constitution ?? item.Constitution;
                item.Intelligence = intelligence ?? item.Intelligence;
                item.Wisdom = wisdom ?? item.Wisdom;
                item.Charisma = charisma ?? item.Charisma;
                item.FortitudeSave = fortitudeSave ?? item.FortitudeSave;
                item.ReflexSave = reflexSave ?? item.ReflexSave;
                item.WillSave = willSave ?? item.WillSave;
                item.Appraise = appraise ?? item.Appraise;
                item.Balance = balance ?? item.Balance;
                item.Bluff = bluff ?? item.Bluff;
                item.Climb = climb ?? item.Climb;
                item.Concentration = concentration ?? item.Concentration;
                item.Craft1 = craft1 ?? item.Craft1;
                item.Craft2 = craft2 ?? item.Craft2;
                item.DecipherScript = decipherScript ?? item.DecipherScript;
                item.Diplomacy = diplomacy ?? item.Diplomacy;
                item.DisableDevice = disableDevice ?? item.DisableDevice;
                item.Disguise = disguise ?? item.Disguise;
                item.EscapeArtist = escapeArtist ?? item.EscapeArtist;
                item.Forgery = forgery ?? item.Forgery;
                item.GatherInformation = gatherInformation ?? item.GatherInformation;
                item.HandleAnimal = handleAnimal ?? item.HandleAnimal;
                item.Heal = heal ?? item.Heal;
                item.Hide = hide ?? item.Hide;
                item.Intimidate = intimidate ?? item.Intimidate;
                item.Jump = jump ?? item.Jump;
                item.KnowledgeArcana = knowledgeArcana ?? item.KnowledgeArcana;
                item.KnowledgeArchitecture = knowledgeArchitecture ?? item.KnowledgeArchitecture;
                item.KnowledgeDungeoneering = knowledgeDungeoneering ?? item.KnowledgeDungeoneering;
                item.KnowledgeHistory = knowledgeHistory ?? item.KnowledgeHistory;
                item.KnowledgeDungeoneering = knowledgeDungeoneering ?? item.KnowledgeDungeoneering;
                item.KnowledgeHistory = knowledgeHistory ?? item.KnowledgeHistory;
                item.KnowledgeLocal = knowledgeLocal ?? item.KnowledgeLocal;
                item.KnowledgeNature = knowledgeNature ?? item.KnowledgeNature;
                item.KnowledgeNobility = knowledgeNobility ?? item.KnowledgeNobility;
                item.KnowledgeThePlanes = knowledgeThePlanes ?? item.KnowledgeThePlanes;
                item.KnowledgeReligion = knowledgeReligion ?? item.KnowledgeReligion;
                item.KnowledgeOther = knowledgeOther ?? item.KnowledgeOther;
                item.Listen = listen ?? item.Listen;
                item.MoveSilently = moveSilently ?? item.MoveSilently;
                item.OpenLock = openLock ?? item.OpenLock;
                item.PerformAct = performAct ?? item.PerformAct;
                item.PerformComedy = performComedy ?? item.PerformComedy;
                item.PerformDance = performDance ?? item.PerformDance;
                item.PerformKeyboard = performKeyboard ?? item.PerformKeyboard;
                item.PerformOratory = performOratory ?? item.PerformOratory;
                item.PerformPercussion = performPercussion ?? item.PerformPercussion;
                item.PerformString = performString ?? item.PerformString;
                item.PerformWind = performWind ?? item.PerformWind;
                item.PerformSing = performSing ?? item.PerformSing;
                item.PerformOther = performOther ?? item.PerformOther;
                item.Profession1 = profession1 ?? item.Profession1;
                item.Profession2 = profession2 ?? item.Profession2;
                item.Ride = ride ?? item.Ride;
                item.Search = search ?? item.Search;
                item.SenseMotive = senseMotive ?? item.SenseMotive;
                item.SleightOfHand = sleightOfHand ?? item.SleightOfHand;
                item.Spellcraft = spellcraft ?? item.Spellcraft;
                item.Spot = spot ?? item.Spot;
                item.Survival = survival ?? item.Survival;
                item.Swim = swim ?? item.Swim;
                item.Tumble = tumble ?? item.Tumble;
                item.UseMagicDevice = useMagicDevice ?? item.UseMagicDevice;
                item.UseRope = useRope ?? item.UseRope;

                //TODO: Implement Proficiency Booleans.

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not save campaign");
                }

                return item.ItemId;
            }
        }

        /// <summary>
        /// Adds a item with the given itemId to a character in the database
        /// </summary>
        /// <param name="itemId">The itemId of the item to be added to the character</param>
        /// <param name="characterId">The characterId of the character to which the item is being added</param>
        /// <returns>The characterId of the modified character (because I didn't know what else to return)</returns>
        public async Task<Guid> AssignItemToCharacterAsync(string itemId, string characterId)
        {
            using (var db = new Dnd35DataContext())
            {
                var items = await db.Items.ToListAsync();
                var item = items.Single(i => i.ItemId.ToString().Equals(itemId));

                item.CharacterId = characterId;

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not update character");
                }

                return item.ItemId;
            }
        }
    }
}
