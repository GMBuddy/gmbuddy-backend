using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
