using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GMBuddy.Games.Micro20.InputModels;

namespace GMBuddy.Games.Micro20.Models
{
    public enum Micro20RaceType
    {
        Human,
        Elf,
        Dwarf,
        Halfling
    }

    public enum Micro20ClassType
    {
        Fighter,
        Rogue,
        Magi,
        Cleric
    }

    public class Character
    {
        public Character()
        {
            
        }

        public Character(NewCharacter model, string userId)
        {
            CampaignId = model.CampaignId;
            UserId = userId;
            Name = model.Name;
            Height = model.Height;
            Weight = model.Weight;
            HairColor = model.HairColor;
            EyeColor = model.EyeColor;
            BaseStrength = model.Strength;
            BaseDexterity = model.Dexterity;
            BaseMind = model.Mind;
            Race = model.Race;
            Class = model.Class;
        }

        [Key]
        public Guid CharacterId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        /// <summary>
        /// Provided by authentication mechanisms (they do not foreign key out to the identity db in any way)
        /// </summary>
        /// <remarks>
        /// UserIds are handled by a different system, so we want them to be opaque strings here
        /// </remarks>
        [Required]
        public string UserId { get; set; }
        
        /// <summary>
        /// A nullable Guid linking this character to 0 or 1 campaign. Can be updated arbitrarily
        /// </summary>
        public Guid? CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }

        [Range(3, 18)]
        public int BaseStrength { get; set; }

        [Range(3, 18)]
        public int BaseDexterity { get; set; }

        [Range(3, 18)]
        public int BaseMind { get; set; }

        [EnumDataType(typeof(Micro20RaceType))]
        public Micro20RaceType Race { get; set; }

        [EnumDataType(typeof(Micro20ClassType))]
        public Micro20ClassType Class { get; set; }

        [Range(1, int.MaxValue)]
        public int Level { get; set; } = 1;
    }
}
