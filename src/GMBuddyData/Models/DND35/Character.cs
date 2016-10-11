using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GMBuddyData.Models.DND35
{
    public class Character
    {
        public Guid CharacterId { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string Name { get; set; }
        public string Bio { get; set; }

        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }
    }
}
