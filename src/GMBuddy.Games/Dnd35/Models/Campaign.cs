using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Campaign
    {
        public Guid CampaignId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string GmEmail { get; set; }
    }
}
