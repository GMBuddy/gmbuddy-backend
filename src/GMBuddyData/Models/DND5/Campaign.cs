using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND5
{
    public class Campaign
    {
        public Guid CampaignId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string GmEmail { get; set; }

        // relationships
        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }
    }
}
