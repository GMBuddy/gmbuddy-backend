using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Games.Dnd35.Models
{
    public class Dnd35Character
    {
        public Guid CharacterId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public Dictionary<string, string> Stats { get; set; }
        public List<Dnd35Item> Items { get; set; }
        public string CampaignId { get; set; }

        //TODO: Implement the actual 3.5 stats
    }
}
