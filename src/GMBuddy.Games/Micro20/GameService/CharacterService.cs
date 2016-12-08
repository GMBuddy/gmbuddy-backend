using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.Data;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.Models;
using GMBuddy.Games.Micro20.OutputModels;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20.GameService
{
    public partial class GameService
    {
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
        public async Task<Guid> CreateCharacter(NewCharacter model, string userId, bool shouldValidate = false)
        {
            if (model == null || string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(model), "Character and user ID must not be null");
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
        /// Modifies a character with any CharacterModification fields that are not null
        /// </summary>
        /// <param name="characterId">The character ID of the character to update</param>
        /// <param name="model">The parameters to update</param>
        /// <param name="userId">Ensures that the given user is allowed to modify the character</param>
        /// <param name="shouldValidate">Whether or not to explicitly validate the DataAnnotations of the model</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and model is invalid</exception>
        /// <exception cref="UnauthorizedException">If the character modification is valid but the user is not allowed to make that modification</exception>
        /// <returns>True if the any properties were changed, false otherwise</returns>
        public async Task<bool> ModifyCharacter(Guid characterId, CharacterModification model, string userId, bool shouldValidate = false)
        {
            if (model == null || string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(model), "The modifications and user ID must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == characterId);
                if (character == null)
                {
                    throw new DataNotFoundException("Could not find the character given by CharacterId");
                }

                if (character.UserId != userId)
                {
                    throw new UnauthorizedException($"User {userId} is not the owner of character {character.CharacterId}");
                }

                // update properties only if they are not null
                character.Name = model.Name ?? character.Name;
                character.Height = model.Height ?? character.Height;
                character.Weight = model.Weight ?? character.Weight;
                character.HairColor = model.HairColor ?? character.HairColor;
                character.EyeColor = model.EyeColor ?? character.EyeColor;
                character.BaseStrength = model.Strength ?? character.BaseStrength;
                character.BaseDexterity = model.Dexterity ?? character.BaseDexterity;
                character.BaseMind = model.Mind ?? character.BaseMind;
                character.Level = model.Level ?? character.Level;

                int changes = await db.SaveChangesAsync();
                return changes == 1;
            }
        }

        /// <summary>
        /// Gets a list of character sheets for the given user's characters
        /// </summary>
        /// <param name="userId">The user requesting their characters</param>
        /// <exception cref="ArgumentNullException">If userId is null or invalid</exception>
        /// <returns></returns>
        public async Task<IEnumerable<CharacterSheet>> ListCharacters(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID invalid");
            }

            using (var db = new DatabaseContext(options))
            {
                var characters = await db.Characters
                    .Where(c => c.UserId == userId)
                    .ToListAsync();
                return characters.Select(c => new CharacterSheet(c));
            }
        }

        /// <summary>
        /// Gets a model sheet for a model of a given ID
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="userId">Ensures that the given user is allowed to view the character</param>
        /// <exception cref="ArgumentException">If characterId is null or empty</exception>
        /// <exception cref="DataNotFoundException">If the given character can not be found</exception>
        /// <exception cref="UnauthorizedException">If the character exists but the user is not allowed to view it</exception>
        /// <returns></returns>
        public async Task<CharacterSheet> GetCharacter(Guid characterId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID invalid");
            }

            using (var db = new DatabaseContext(options))
            {
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == characterId);
                if (character == null)
                {
                    throw new DataNotFoundException("Character could not be found");
                }

                if (character.UserId != userId)
                {
                    throw new UnauthorizedException("User is not authorized to view character");
                }

                return new CharacterSheet(character); ;
            }
        }
    }
}
