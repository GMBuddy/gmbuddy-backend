using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class CharacterModification
    {
        [Required]
        public Guid? CharacterId { get; set; }
        
        public string Name { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        public Guid? CampaignId { get; set; }

        [Range(3, 18)]
        public int? Strength { get; set; }

        [Range(3, 18)]
        public int? Dexterity { get; set; }

        [Range(3, 18)]
        public int? Mind { get; set; }

        [Range(0,100)] //assuming a level 20 cap
        public int? Physical { get; set; }

        [Range(0,100)] //assuming a level 20 cap
        public int? Subterfuge { get; set; }

        [Range(0,100)] //assuming a level 20 cap
        public int? Knowledge { get; set; }

        [Range(0,100)] //assuming a level 20 cap
        public int? Communication { get; set; }

        [Range(1, int.MaxValue)]
        public int? Level { get; set; }

        [Range(1, int.MaxValue)]
        public int? Experience { get; set;} //when modifying a character, if experience is ever equal or greater than 10xlevel, player advances a level, then experience resets (extra exp rolls over)

        internal ICollection<Item> Items { get; set; }

        internal ICollection<Spell> Spells { get; set; }
 
        [Range(0, int.MaxValue)]
        public int? CopperPieces { get; set; }

        [Range(0, int.MaxValue)]
        public int? SilverPieces { get; set; }

        [Range(0, int.MaxValue)]
        public int? GoldPieces { get; set; }

        [Range(0, int.MaxValue)]
        public int? PlatinumPieces { get; set; }
    }
}
