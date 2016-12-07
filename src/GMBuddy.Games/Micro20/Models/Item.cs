using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Micro20.Models
{
    public class Item
    {
        public enum Micro20ItemType
        {
            Equipment,
            Weapon,
            Armor
        }

        [Key]
        public Guid ItemID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Cost { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, EnumDataType(typeof(Micro20ItemType))]
        public Micro20ItemType ItemType { get; set; }

        public string WeaponDamage { get; set; } //ItemType.Weapon should have Damage

        public string WeaponRange { get; set; } //ItemType.Weapons should have Range

        public string ArmorBonus { get; set; } //ItemType.Armor should have ArmorBonus
    }
}