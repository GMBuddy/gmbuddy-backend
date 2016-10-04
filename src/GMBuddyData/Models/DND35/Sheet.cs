using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Sheet : ICharacterAttributes
    {
        public Guid SheetId { get; set; }
        public Guid CampaignCharacterId { get; set; }
        public CampaignCharacter CampaignCharacter { get; set; }

        public ICollection<Item> Items { get; set; }

        // Implement character attributes
        public int Strength { get; set; }
        public int Charisma { get; set; }
        public int Endurance { get; set; }
    }
}
