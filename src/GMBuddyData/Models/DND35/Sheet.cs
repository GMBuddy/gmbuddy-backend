using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Sheet : CharacterAttributes
    {
        public Guid CampaignCharacterId { get; set; }
        public CampaignCharacter CampaignCharacter { get; set; }
    }
}
