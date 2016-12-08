using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GMBuddy.Games.Micro20.Models;
using System.Collections.Generic;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class NewCharacter
    {
        public Guid? CampaignId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        [Range(3, 18)]
        public int Strength { get; set; }

        [Range(3, 18)]
        public int Dexterity { get; set; }

        [Range(3, 18)]
        public int Mind { get; set; }

        [Range(1,23)] //assuming a level 20 cap
        public int Physical { get; set; }

        [Range(1,23)] //assuming a level 20 cap
        public int Subterfuge { get; set; }

        [Range(1,23)] //assuming a level 20 cap
        public int Knowledge { get; set; }

        [Range(1,23)] //assuming a level 20 cap
        public int Communication { get; set; }

        [EnumDataType(typeof(Micro20RaceType))]
        public Micro20RaceType Race { get; set; }

        [EnumDataType(typeof(Micro20ClassType))]
        public Micro20ClassType Class { get; set; }

        [Range(1, int.MaxValue)]
        public int Level { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int Experience { get; set; } = 0;
        
        internal ICollection<Item> Items { get; set; }

        internal ICollection<Spell> Spells { get; set; }

        [Range(0, int.MaxValue)]
        public int CopperPieces { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int SilverPieces { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int GoldPieces { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int PlatinumPieces { get; set; } = 0;
    }
}
