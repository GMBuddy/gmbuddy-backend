using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Item : ICharacterAbilities
    {
        public Guid ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public ItemType ItemType { get; set; }
        public string ItemDescription { get; set; }

        public Guid CampaignCharacterId { get; set; }
        public CampaignCharacter CampaignCharacter { get; set; }

        public Guid CharacterId { get; set; }
        public Character Character { get; set; }

        // ICharacterAbilities implementation
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Generic
    }
}
