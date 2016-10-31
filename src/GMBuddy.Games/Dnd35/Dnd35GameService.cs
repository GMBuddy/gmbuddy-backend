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
        /// <param name="bio">The biography of the character to be created</param>
        /// <returns>The ID of the added character</returns>
        public async Task<Guid> AddCharacterAsync(string userId, string name, string bio)
        {
            using (var db = new Dnd35DataContext())
            {
                var character = new Dnd35Character
                {
                    UserId = userId,
                    Name = name,
                    Bio = bio
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
        /// <param name="userId">The email of the user adding the stat to the character</param>
        /// <param name="statName">The name of the stat to be added</param>
        /// <param name="statValue">The value of the stat to be added</param>
        /// <param name="characterId">The characterId of the character to which the stat is being added</param>
        /// <param name="campaignId">The campaignId of the campaign which the character is in</param>
        /// All the oher parameters are able to be null, and they are the values of the attributes to be modified
        /// <returns>The characterId of the modified character (because I didn't know what else to return)</returns>
        public async Task<Guid> ModifyAttributesAsync(string userId, string characterId, string characterClass,
        int level, string race, string size, string gender, string allignment,
        string diety, string height, string weight, string looks, string[] languages, string[] feats,
        string[] racialTraitsAndFeatures, int spellSaveDC, int carryCapacityLight, int carryCapacityMedium,
        int carryCapacityHeavy, int experience, int normalAC, int touchAC, int flatootedAC, int maxHitpoints,
        int currentHitpoints, int strength, int dexterity, int constitution, int intelligence, int wisdom,
        int charisma, int fortitudeSave, int reflexSave, int willSave, int appraise, int balance, int bluff,
        int climb, int concentration, int craft1, string craft1Type, int craft2, string craft2Type, int craft3,
        string craft3Type, int decipherScript, int diplomacy, int disableDevice, int disguise, int escapeArtist,
        int forgery, int gatherInformation, int handleAnimal, int heal, int hide, int intimidate, int jump,
        int knowledgeArcana, int knowledgeArchitecture, int knowledgeDungeoneering, int knowledgeHistory,
        int knowledgeLocal, int knowledgeNature, int knowledgeNobility, int knowledgeThePlanes, int knowledgeReligion,
        int knowledgeOther, string knowledgeOtherType, int listen, int moveSilently, int openLock, int performAct,
        int performComedy, int performDance, int performKeyboard, int performOratory, int performPercussion,
        int performString, int performWind, int performSing, int performOther, string performOtherType, int profession1,
        string profession1Type, int profession2, string profession2Type, int ride, int search, int senseMotive,
        int sleightOfHand, int spellcraft, int spot, int survival, int swim, int tumble, int useMagicDevice,
        int useRope)
        {
            using (var db = new Dnd35DataContext())
            {
                //get the character, add the key value pair to the character's stat dictionary
                var characters = await db.Characters.ToListAsync();
                var character = characters.Single(c => (c.CharacterId.ToString().Equals(characterId)) && (c.CampaignId.ToString().Equals(campaignId)));

                if(character.UserId != userId)
                {
                    throw new Exception("Current user does not have access to modify this campaign"); //Should we even check for this? Should we also allow GM to modify a player's stats?
                }
                if(characterClass != null)
                  character.Class = characterClass;
                if(level != null)
                  character.Level = level;
                if(race != null)
                  character.Race = race;
                if(size != null)
                  character.Size = size;
                if(gender != null)
                  character.Gender = gender;
                if(allignment != null)
                  character.Allignment = allignment;
                if(diety != null)
                  character.Diety = diety;
                if(height != null)
                  character.Height = height;
                if(weight!= null)
                  character.Weight = weight;
                if(looks != null)
                  character.Looks = looks;
                if(languages != null){
                  foreach(string language in languages){
                    character.Languages.add(language);
                  }
                }
                if(feats != null){
                  foreach(string feat in feats){
                    characters.Feats.add(feat);
                  }
                }
                if(racialTraitsAndFeatures != null){
                  foreach(string trait in racialTraitsAndFeatures){
                    characters.RacialTraitsAndClassFeatures.add(trait);
                  }
                }
                character.SpellSaveDC = spellSaveDC ?? character.SpellSaveDC;
                //TODO: Change the ones below to look like the one above!
                if(carryCapacityLight != null)
                  character.CarryCapacityLight = carryCapacityLight;
                if(carryCapacityMedium != null)
                  character.CarryCapacityMedium = carryCapacityMedium;
                if(carryCapacityHeavy != null)
                  character.CarryCapacityHeavy = carryCapacityHeavy;
                if(experience != null)
                  character.Experience = experience;
                if(normalAC != null)
                  character.AC = normalAC;
                if(touchAC != null)
                  character.TouchAC = touchAC;
                if(flatFootedAC != null)
                  character.FlatFootedAC = flatootedAC;
                if(maxHitpoints != null)
                  character.MaxHitpoints = maxHitpoints;
                if(currentHitpoints != null)
                  character.CurrentHitpoints = currentHitpoints;
                if(strength != null)
                  character.Strength = strength;
                if(dexterity != null)
                  character.Dexterity = dexterity;
                if(constitution != null)
                  character.Constitution = constitution;
                if(interlligence != null)
                  character.Intelligence = intelligence;
                if(wisdom != null)
                  character.Wisdom = wisdom;
                if(charisma != null)
                  character.Charisma = charisma;
                if(fortitudeSave != null)
                  character.fortitudeSave = FortitudeSave;
                if(reflexSave != null)
                  character.ReflexSave = reflexSave;
                if(willSave != null)
                  character.WillSave = willSave;
                if(appraise != null)
                  character.Appraise = appraise;
                if(balance != null)
                  character.Balance = balance;
                if(bluff != null)
                  character.Bluff = bluff;
                if(climb != null)
                  character.Climb = climb;
                if(concentration != null)
                  character.Concentration = concentration;
                if(craft1 != null)
                  character.Craft1 = craft1;
                if(craft1Type != null)
                  character.Craft1Type = craft1Type;
                if(craft2 != null)
                  character.Craft2 = craft2;
                if(craft2Type != null)
                  character.Craft2Type = craft2Type;
                if(decipherScript != null)
                  character.DecipherScript = decipherScript;
                if(diplomacy != null)
                  character.Diplomacy = diplomacy;
                if(disableDevice != null)
                  character.DisableDevice = disableDevice;
                if(disguise != null)
                  character.Disguise = disguise;
                if(escapeArtist != null)
                  chaaracter.EscapeArtist = escapeArtist;
                if(forgery != null)
                  character.Forgery = forgery;
                if(gatherInformation != null)
                  character.GatherInformation = gatherInformation;
                if(handleAnimal != null)
                  character.HandleAnimal = handleAnimal;
                if(heal != null)
                  character.Heal = heal;
                if(hide != null)
                  character.Hide = hide;
                if(intimidate != null)
                  character.Intimidate = intimidate;
                if(jump != null)
                  character.Jump = jump;
                if(knowledgeArcana != null)
                  character.KnowledgeArcana = knowledgeArcana;
                if(knowledgeArchitecture != null)
                  character.KnowledgeArchitecture = knowledgeArchitecture;
                if(knowledgeDungeoneering != null)
                  character.KnowledgeDungeoneering = knowledgeDungeoneering;
                if(knowledgeHistory != null)
                  character.KnowledgeHistory = knowledgeHistory;
                if(knowledgeDungeoneering != null)
                  character.KnowledgeDungeoneering = knowledgeDungeoneering;
                if(knowledgeHistory != null)
                  character.KnowledgeHistory = knowledgeHistory;
                if(knowledgeLocal != null)
                  character.KnowledgeLocal = KnowledgeLocal;
                if(knowledgeNature != null)
                  character.KnowledgeNature = knowledgeNature;
                if(knowledgeNobility != null)
                  character.KnowledgeNobility = knowledgeNobility;
                if(knowledgeThePlanes != null)
                  character.KnowledgeThePlanes = knowledgeThePlanes;
                if(knowledgeReligion != null)
                  character.KnowledgeReligion = knowledgeReligion;
                if(knowledgeOther != null)
                  character.KnowledgeOther = knowledgeOther;
                if(knowledgeOtherType != null)
                  character.KnowledgeOtherType = knowledgeOtherType;
                if(listen != null)
                  character.Listen = listen;
                if(moveSilently != null)
                  character.MoveSilently = moveSilently;
                if(openLock != null)
                  character.OpenLock = openLock;
                if(performAct != null)
                  character.PerformAct = performAct;
                if(performComedy != null)
                  character.PerformAct = performAct;
                if(performDance != null)
                  character.PerformDance = performDance;
                if(performKeyboard != null)
                  character.PerformKeyboard = performKeyboard;
                if(performOratory != null)
                  character.PerformOratory = performOratory;
                if(performPercussion != null)
                  character.PerformPercussion = performPercussion;
                if(performString != null)
                  character.PerformString = performString;
                if(performWind != null)
                  character.PerformWind = performWind;
                if(performSing != null)
                  character.PerformSing = performSing;
                if(performOther != null)
                  character.PerformOther = performOther;
                if(performOtherType != null)
                  character.PerformOtherType = performOtherType;
                if(profession1 != null)
                  character.Profession1 = profession1;
                if(profession1Type != null)
                  character.Profession1Type = profession1Type;
                if(profession2 != null)
                  character.Profession2 = profession2;
                if(profession2Type != null)
                  character.Profession2Type = profession2Type;
                if(ride != null)
                  character.Ride = ride;
                if(search != null)
                  character.Search = search;
                if(senseMotive != null)
                  character.SenseMotive = senseMotive;
                if(sleightOfHand != null)
                  character.SleightOfHand = sleightOfHand;
                if(spellcraft != null)
                  character.Spellcraft = spellcraft;
                if(spot != null)
                  character.Spot = spot;
                if(survival != null)
                  character.Survival = survival;
                if(swim != null)
                  character.Swim = swim;
                if(tumble != null)
                  character.Tumble = tumble;
                if(useMagicDevice != null)
                  character.UseMagicDevice = useMagicDevice;
                if(useRope != null)
                  character.UseRope = useRope;

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

                character.ChracterId = characterId;

                int changes = await db.SaveChangesAsync();

                if (changes != 1)
                {
                    throw new Exception("Could not update character");
                }

                return character.CharacterId;
            }
        }
    }
}
