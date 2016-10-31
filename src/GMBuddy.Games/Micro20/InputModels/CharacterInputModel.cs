using System.ComponentModel.DataAnnotations;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class CharacterInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string CampaignId { get; set; }

        [Range(3, 18)]
        public int Strength { get; set; }

        [Range(3, 18)]
        public int Dexterity { get; set; }

        [Range(3, 18)]
        public int Mind { get; set; }

        public Micro20RaceType Race { get; set; }

        public Micro20ClassType Class { get; set; }
    }
}
