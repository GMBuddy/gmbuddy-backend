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

        public Guid SheetId { get; set; }
        public Sheet Sheet { get; set; }
        
        public int Strength { get; set; }
        public int Charisma { get; set; }
        public int Endurance { get; set; }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion,
        Generic
    }
}
