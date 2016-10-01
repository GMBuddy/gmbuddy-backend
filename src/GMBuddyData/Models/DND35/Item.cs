using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Item : CharacterAttributes
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        public string ItemDescription { get; set; }

        public Guid CampaignCharacterId { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Generic
    }
}
