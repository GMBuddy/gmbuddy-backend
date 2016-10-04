using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class CampaignCharacter
    {
        public Guid CampaignCharacterId { get; set; }
        public bool IsGameMaster { get; set; }

        public Guid CharacterId { get; set; }
        public Character Character { get; set; }

        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        
        public Sheet Sheet { get; set; }
     }
}
