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
        /// Modifies a character with the given CharacterModification fields
        /// </summary>
        /// <param name="model">The parameters to update</param>
        /// <param name="userId">Ensures that the given user is allowed to modify the character</param>
        /// <param name="shouldValidate">If shouldValidate = true and the given model isnt valid</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and model is invalid</exception>
        /// <exception cref="UnauthorizedException">If the character modification is valid but the user is not allowed to make that modification</exception>
        /// <returns>True if the any properties were changed, false otherwise</returns>
        public async Task<bool> ModifyCharacter(CharacterModification model, string userId, bool shouldValidate = false)
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
                var character = await db.Characters.SingleOrDefaultAsync(c => c.CharacterId == model.CharacterId);
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
                character.CampaignId = model.CampaignId ?? character.CampaignId;
                character.BaseStrength = model.Strength ?? character.BaseStrength;
                character.BaseDexterity = model.Dexterity ?? character.BaseDexterity;
                character.BaseMind = model.Mind ?? character.BaseMind;
                character.Experience = model.Experience ?? character.Experience;
                if (character.Experience >= 10 * character.Level)
                {
                    character.Experience -= 10 * character.Level;
                    character.Level++;                 
                }
                character.Items = model.Items ?? character.Items; //is this how the front end team wants items to be modified, or should there be seperate methods for adding and removing an item from a character?
                character.Spells = model.Spells ?? character.Spells;
                character.CopperPieces = model.CopperPieces ?? character.CopperPieces;
                character.SilverPieces = model.SilverPieces ?? character.SilverPieces;
                character.GoldPieces = model.GoldPieces ?? character.GoldPieces;
                character.PlatinumPieces = model.PlatinumPieces ?? character.PlatinumPieces;


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

        /// <summary>
        /// Creates a new item associated with all campaigns and characters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shouldValidate">Whether to explicitly validate the given model</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and the given model is invalid</exception>
        /// <exception cref="DataNotCreatedException">If the item was not added to the database</exception>
        /// <returns>The item's ID</returns>
        public async Task<Guid> CreateItem(NewItem model, bool shouldValidate = false)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Item must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var item = new Item(model);

                db.Items.Add(item);

                int changes = await db.SaveChangesAsync();
                if (changes != 1)
                {
                    throw new DataNotCreatedException("Could not add item to database");
                }

                return item.ItemId;
            }
        }

        /// <summary>
        /// Modifies an item with the given ItemModification fields
        /// </summary>
        /// <param name="model">The parameters to update</param>
        /// <param name="shouldValidate">If shouldValidate = true and the given model isnt valid</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and model is invalid</exception>
        /// <returns>True if the any properties were changed, false otherwise</returns>
        public async Task<bool> ModifyItem(ItemModification model, bool shouldValidate = false)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The modifications must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var item = await db.Items.SingleOrDefaultAsync(i => i.ItemId == model.ItemId);
                if (item == null)
                {
                    throw new DataNotFoundException("Could not find the item given by itemId");
                }

                // update properties only if they are not null
                item.Name = model.Name ?? item.Name;
                item.Cost = model.Cost ?? item.Cost;
                item.Description = model.Description ?? item.Description;
                if (item.ItemType == Micro20ItemType.Weapon)
                {
                    item.WeaponDamage = model.WeaponDamage ?? item.WeaponDamage;
                    item.WeaponRange = model.WeaponRange ?? item.WeaponRange;
                }
                else if (item.ItemType == Micro20ItemType.Armor)
                {
                    item.ArmorBonus = model.ArmorBonus ?? item.ArmorBonus;
                }

                int changes = await db.SaveChangesAsync();
                return changes == 1;
            }
        }

        /// <summary>
        /// Gets a list of all the items in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Item>> ListItems()
        {
            using (var db = new DatabaseContext(options))
            {
                return await db.Items.ToListAsync();
            }
        }

        /// <summary>
        /// Gets a list of all the weapons in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Item>> ListWeapons()
        {
            using (var db = new DatabaseContext(options))
            {
                var weapons = await db.Items
                    .Where(i => i.ItemType == Micro20ItemType.Weapon)
                    .ToListAsync();
                return weapons;
            }
        }

        /// <summary>
        /// Gets a list of all the armor in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Item>> ListArmor()
        {
            using (var db = new DatabaseContext(options))
            {
                var armor = await db.Items
                    .Where(i => i.ItemType == Micro20ItemType.Armor)
                    .ToListAsync();
                return armor;
            }
        }

        /// <summary>
        /// Gets a list of all the equipment in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Item>> ListEquipment()
        {
            using (var db = new DatabaseContext(options))
            {
                var equipment = await db.Items
                    .Where(i => i.ItemType == Micro20ItemType.Equipment)
                    .ToListAsync();
                return equipment;
            }
        }

        /// <summary>
        /// Gets the model of a specific item
        /// </summary>
        /// <param name="itemId"></param>
        /// <exception cref="DataNotFoundException">If the given item can not be found</exception>
        /// <returns></returns>
        public async Task<Item> GetItem(Guid itemId)
        {
            using (var db = new DatabaseContext(options))
            {
                var item = await db.Items.SingleOrDefaultAsync(i => i.ItemId == itemId);
                if (item == null)
                {
                    throw new DataNotFoundException("Item could not be found");
                }

                return item;
            }
        }

        /// <summary>
        /// Creates a new spell associated with all campaigns and characters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shouldValidate">Whether to explicitly validate the given model</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="DataNotCreatedException">If the spell was not added to the database</exception>
        /// <returns>The spell's ID</returns>
        public async Task<Guid> CreateSpell(NewSpell model, bool shouldValidate = false)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Spell must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var spell = new Spell(model);

                db.Spells.Add(spell);

                int changes = await db.SaveChangesAsync();
                if (changes != 1)
                {
                    throw new DataNotCreatedException("Could not add spell to database");
                }

                return spell.SpellId;
            }
        }

        /// <summary>
        /// Modifies a spell with the given SpellModification fields
        /// </summary>
        /// <param name="model">The parameters to update</param>
        /// <param name="shouldValidate">If shouldValidate = true and the given model isnt valid</param>
        /// <exception cref="ArgumentNullException">If model is empty</exception>
        /// <exception cref="ValidationException">If shouldValidate = true and model is invalid</exception>
        /// <returns>True if the any properties were changed, false otherwise</returns>
        public async Task<bool> ModifySpell(SpellModification model, bool shouldValidate = false)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The modifications must not be null");
            }

            if (shouldValidate)
            {
                Validator.ValidateObject(model, new ValidationContext(model), true);
            }

            using (var db = new DatabaseContext(options))
            {
                var spell = await db.Spells.SingleOrDefaultAsync(s => s.SpellId == model.SpellId);
                if (spell == null)
                {
                    throw new DataNotFoundException("Could not find the spell given by spellId");
                }

                // update properties only if they are not null
                spell.Name = model.Name ?? spell.Name;
                spell.School = model.School ?? spell.School;
                spell.Level = model.Level ?? spell.Level;

                int changes = await db.SaveChangesAsync();
                return changes == 1;
            }
        }

        /// <summary>
        /// Gets a list of all the spells in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Spell>> ListSpells()
        {
            using (var db = new DatabaseContext(options))
            {
                var spells = await db.Spells
                    .ToListAsync();
                return spells;
            }
        }

        /// <summary>
        /// Gets the model of a specific spell
        /// </summary>
        /// <param name="spellId"></param>
        /// <exception cref="ArgumentException">If spellId is null or empty</exception>
        /// <exception cref="DataNotFoundException">If the given spell can not be found</exception>
        /// <returns></returns>
        public async Task<Spell> GetSpell(Guid spellId)
        {
            using (var db = new DatabaseContext(options))
            {
                var spell = await db.Spells.SingleOrDefaultAsync(s => s.SpellId == spellId);
                if (spell == null)
                {
                    throw new DataNotFoundException("Spell could not be found");
                }

                return spell;
            }
        }
    }
}
