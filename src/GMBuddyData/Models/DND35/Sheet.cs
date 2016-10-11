using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35
{
    public class Sheet : ICharacterAbilities
    {
        public Guid SheetId { get; set; }
        public Guid CampaignCharacterId { get; set; }
        public CampaignCharacter CampaignCharacter { get; set; }

        public ICollection<Item> Items { get; set; }

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Constitution
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Intelligence
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Wisdom
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Charisma
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        // Implement character attributes
    }
}
