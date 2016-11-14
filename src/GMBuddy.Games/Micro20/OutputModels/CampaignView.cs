using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Models;

namespace GMBuddy.Games.Micro20.OutputModels
{
    public class CampaignView
    {
        private readonly Campaign campaign;

        public CampaignView(Campaign c)
        {
            campaign = c;
        }

        public Guid CampaignId => campaign.CampaignId;

        public string Name => campaign.Name;

        public string GmUserId => campaign.GmUserId;

        public IEnumerable<CharacterSheet> Characters => campaign.Characters?.Select(c => new CharacterSheet(c));
    }
}
