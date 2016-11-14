using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GMBuddy.Games.Micro20.Models
{
    public class Campaign
    {
        [Key]
        public Guid CampaignId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GmUserId { get; set; }

        [InverseProperty("Campaign")]
        public ICollection<Character> Characters { get; set; }
    }
}
