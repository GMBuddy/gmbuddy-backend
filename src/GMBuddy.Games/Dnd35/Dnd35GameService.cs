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
        /// <returns>The characterId of the modified character (because I didn't know what else to return)</returns>
        public async Task<Guid> AddStatToCharacterAsync(string userId, string statName, string statValue, string characterId, string campaignId)
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
                
                character.Stats.Add(statName, statValue);
                
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
                var characters = await db.Characters.ToListAsync();
                var character = characters.Single(c => c.CharacterId.ToString().Equals(characterId));

                var items = await db.Items.ToListAsync();
                var item = items.Single(i => i.ItemId.ToString().Equals(itemId));
                
                character.Items.Add(item);
                
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
