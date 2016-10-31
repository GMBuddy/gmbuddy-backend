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

        public Character(CharacterInputModel model)
        {
            CampaignId = Guid.Parse(model.CampaignId);
            UserId = model.UserId;
            BaseStrength = model.Strength;
            BaseDexterity = model.Dexterity;
            BaseMind = model.Mind;
            Race = model.Race;
            Class = model.Class;
        }

        [Key]
        public Guid CharacterId { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public Guid CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Micro20Campaign Campaign { get; set; }

        [Range(3, 18)]
        public int BaseStrength { get; set; }

        [Range(3, 18)]
        public int BaseDexterity { get; set; }

        [Range(3, 18)]
        public int BaseMind { get; set; }

        public Micro20RaceType Race { get; set; }

        public Micro20ClassType Class { get; set; }

        public int Level { get; set; } = 1;
    }
}
