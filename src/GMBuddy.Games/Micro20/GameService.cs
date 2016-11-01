using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20
{
    public class GameService
    {
        private readonly DbContextOptions options;

        public GameService(DbContextOptions options = null)
        {
            this.options = options ?? new DbContextOptionsBuilder().Options;
        }

        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Campaign>> GetCampaigns()
        {
            using (var db = new DatabaseContext(options))
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
        public async Task<Guid> AddCampaign(string name, string userId)
        {
            using (var db = new DatabaseContext(options))
            {
                var campaign = new Campaign
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
        /// Creates a new character associated with a single campaign
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="shouldValidate">Whether to explicitly validate the given model</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and the given model is invalid</exception>
        /// <exception cref="DataNotCreatedException">If the character was not added to the database</exception>
        /// <returns>The character's ID</returns>
        public async Task<Guid> AddCharacter(NewCharacter model, string userId, bool shouldValidate = false)
        {
            if (model == null || string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(model), "Character must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var character = new Character(model, userId);

                db.Characters.Add(character);

                int changes = await db.SaveChangesAsync();
                if (changes != 1)
                {
                    throw new DataNotCreatedException("Could not add character to campaign");
                }

                return character.CharacterId;
            }
        }

        /// <summary>
        /// Modifies a character with the given CharacterModification fields
        /// </summary>
        /// <param name="model">The parameters to update</param>
        /// <param name="shouldValidate">If shouldValidate = true and the given model isnt valid</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and model is invalid</exception>
        /// <returns>True if the any properties were changed, false otherwise</returns>
        public async Task<bool> ModifyCharacter(CharacterModification model, bool shouldValidate = false)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The CharacterModification must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == model.CharacterId);
                if (character == null)
                {
                    throw new DataNotFoundException("Could not find the character given by CharacterId");
                }

                // update properties only if they are not null
                character.BaseStrength = model.Strength ?? character.BaseStrength;
                character.BaseDexterity = model.Dexterity ?? character.BaseDexterity;
                character.BaseMind = model.Mind ?? character.BaseMind;
                character.Level = model.Level ?? character.Level;

                int changes = await db.SaveChangesAsync();
                return changes == 1;
            }
        }

        /// <summary>
        /// Gets a model sheet for a model of a given ID
        /// </summary>
        /// <param name="characterId"></param>
        /// <exception cref="ArgumentException">If characterId is null or empty</exception>
        /// <exception cref="DataNotFoundException">If the given character can not be found</exception>
        /// <returns></returns>
        public async Task<CharacterSheet> GetSheet(Guid characterId)
        {
            if (characterId == null)
            {
                throw new ArgumentException("Invalid model id", nameof(characterId));
            }

            using (var db = new DatabaseContext(options))
            {
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == characterId);
                if (character == null)
                {
                    throw new DataNotFoundException("Character could not be found");
                }

                return new CharacterSheet(character); ;
            }
        }
    }
}
