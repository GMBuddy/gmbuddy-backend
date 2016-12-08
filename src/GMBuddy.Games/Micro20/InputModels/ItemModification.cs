using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class ItemModification
    {
        [Required]
        public Guid? ItemId { get; set; }
        
        public string Name { get; set; }

        public string Cost { get; set; }

        public string Description { get; set; }

        public string WeaponDamage { get; set; } //ItemType.Weapon should have WeaponDamage

        public string WeaponRange { get; set; } //ItemType.Weapon should have WeaponRange

        public string ArmorBonus { get; set; } //ItemType.Armor should have ArmorBonus
    }
}
