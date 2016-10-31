using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.InputModels
{
    public class NewCharacter
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public Guid CampaignId { get; set; }

        [Range(3, 18)]
        public int Strength { get; set; }

        [Range(3, 18)]
        public int Dexterity { get; set; }

        [Range(3, 18)]
        public int Mind { get; set; }

        [EnumDataType(typeof(Micro20RaceType))]
        public Micro20RaceType Race { get; set; }

        [EnumDataType(typeof(Micro20ClassType))]
        public Micro20ClassType Class { get; set; }
    }
}
