using System;
using System.ComponentModel.DataAnnotations;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Dnd35Campaign
    {
        [Key]
        public Guid CampaignId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GmUserId { get; set; }
    }
}
