using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Campaign
    {
        public Guid CampaignId { get; set; }
        public string Name { get; set; }

        // relationships
        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }
    }
}
