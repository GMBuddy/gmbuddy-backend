using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GMBuddyData.ViewModels;
using Newtonsoft.Json;

namespace GMBuddyData.Models.DND35
{
    public class Character : ICharacterView
    {
        public Guid CharacterId { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string Name { get; set; }

        public string Bio { get; set; }

        [Required]
        [StringLength(20)]
        public string Class { get; set; }

        [Required]
        [StringLength(20)]
        public string Deity { get; set; }

        [Required]
        [StringLength(20)]
        public string Alignment { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; }
        
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        [Required]
        [StringLength(20)]
        public string Size { get; set; }

        [Required]
        [StringLength(20)]
        public string Eyes { get; set; }

        [Required]
        [StringLength(20)]
        public string Skin { get; set; }

        [JsonIgnore]
        [InverseProperty("Character")]
        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }

        [JsonIgnore]
        [InverseProperty("Character")]
        public ICollection<Item> StartingItems { get; set; }

        // ICharacterAbility implementation
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        // ICharacterView implementation
        public ICollection<Item> Items
        {
            get
            {
                var items = new List<Item>(StartingItems);
                items.AddRange(CampaignCharacters.SelectMany(cc => cc.Items));
                return items;
            }
            set { throw new NotSupportedException(); }
        }
    }
}
