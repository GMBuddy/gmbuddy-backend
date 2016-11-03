using System;
using System.ComponentModel.DataAnnotations;

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

        public Guid? Campaign { get; set; }

        [Range(3, 18)]
        public int? Strength { get; set; }

        [Range(3, 18)]
        public int? Dexterity { get; set; }

        [Range(3, 18)]
        public int? Mind { get; set; }

        [Range(1, 100)]
        public int? Level { get; set; }
    }
}
