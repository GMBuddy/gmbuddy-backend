using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GMBuddyData.ViewModels;
using Newtonsoft.Json;

namespace GMBuddyData.Models.DND35
{
    public class Campaign : ICampaignView
    {
        public Guid CampaignId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string GmEmail { get; set; }

        // relationships
        [JsonIgnore]
        public ICollection<CampaignCharacter> CampaignCharacters { get; set; }

        // ICampaign implementation
        [NotMapped]
        public IEnumerable<IBasicCharacterView> Characters
        {
            get { return CampaignCharacters?.Select(cc => cc.Character); }
            set { throw new NotSupportedException(); }
        }
    }
}
