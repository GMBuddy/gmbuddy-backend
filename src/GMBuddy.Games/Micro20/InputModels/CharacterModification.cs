using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class CharacterModification
    {
        [Required]
        public Guid? CharacterId { get; set; }

        public Guid? NewCampaign { get; set; }

        [Range(3, 18)]
        public int? NewStrength { get; set; }

        [Range(3, 18)]
        public int? NewDexterity { get; set; }

        [Range(3, 18)]
        public int? NewMind { get; set; }

        [Range(1, 100)]
        public int? NewLevel { get; set; }
    }
}
