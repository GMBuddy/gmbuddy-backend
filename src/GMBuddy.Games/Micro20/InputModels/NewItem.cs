using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class NewItem
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Cost { get; set; }

        public string WeaponDamage { get; set; } //ItemType.Weapon should have WeaponDamage

        public string WeaponRange { get; set; } //ItemType.Weapon should have WeaponRange

        public string ArmorBonus { get; set; } //ItemType.Armor should have ArmorBonus

        [Required, EnumDataType(typeof(Micro20ItemType))]
        public Micro20ItemType ItemType { get; set; }
    }
}
