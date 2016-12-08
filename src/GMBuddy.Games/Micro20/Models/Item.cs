using System;
using System.ComponentModel.DataAnnotations;
using GMBuddy.Games.Micro20.InputModels;

namespace GMBuddy.Games.Micro20.Models
{
    public enum Micro20ItemType
    {
        Equipment,
        Weapon,
        Armor
    }

    public class Item
    {
        public Item()
        {
            
        }

        public Item(NewItem model)
        {
            Name = model.Name;
            Cost = model.Cost;
            Description = model.Description;
            ItemType = model.ItemType;
            WeaponDamage = model.WeaponDamage;
            WeaponRange = model.WeaponRange;
            ArmorBonus = model.ArmorBonus;
        }

        [Key]
        public Guid ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Cost { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, EnumDataType(typeof(Micro20ItemType))]
        public Micro20ItemType? ItemType { get; set; } //nullable so it is actually required and not defaulting to 0 if null

        public string WeaponDamage { get; set; } //ItemType.Weapon should have Damage

        public string WeaponRange { get; set; } //ItemType.Weapons should have Range

        public string ArmorBonus { get; set; } //ItemType.Armor should have ArmorBonus
    }
}