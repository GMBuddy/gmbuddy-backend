using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20
{
    public class Micro20GameService
    {
        private readonly DbContextOptions options;

        public Micro20GameService(DbContextOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Get all campaigns (eventually with filtering options)
        /// </summary>
        /// <returns>Returns a list of campaigns. If none exist, an empty array is returned. If an error occurs, an exception is thrown</returns>
        public async Task<IEnumerable<Micro20Campaign>> GetCampaigns()
        {
            using (var db = new Micro20DataContext(options))
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
            using (var db = new Micro20DataContext(options))
            {
                var campaign = new Micro20Campaign
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
        /// <param name="input"></param>
        /// <param name="shouldValidate">Whether to explicitly validate the given input</param>
        /// <returns>The character's ID</returns>
        public async Task<Guid> AddCharacter(CharacterInputModel input, bool shouldValidate = false)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Character must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(input, new ValidationContext(input), true);
            }

            using (var db = new Micro20DataContext(options))
            {
                var character = new Character(input);

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
        /// Gets a input sheet for a input of a given ID
        /// </summary>
        /// <exception cref="ArgumentException">If the given input id is null or invalid</exception>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public async Task<Micro20CharacterSheet> GetSheet(string characterId)
        {
            if (string.IsNullOrWhiteSpace(characterId))
            {
                throw new ArgumentException("Invalid input id", nameof(characterId));
            }

            using (var db = new Micro20DataContext(options))
            {
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId.ToString() == characterId);
                if (character == null)
                {
                    throw new DataNotFoundException("Character could not be found");
                }

                return new Micro20CharacterSheet(character); ;
            }
        }
    }
}
