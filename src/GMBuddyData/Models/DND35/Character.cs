using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Character
    {
        public Guid CharacterId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }
    }
}
