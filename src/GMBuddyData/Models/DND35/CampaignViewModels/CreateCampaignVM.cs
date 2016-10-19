using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddyData.Models.DND35.CampaignViewModels
{
    public class CreateCampaignVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string GameMaster { get; set; }
        //TODO: Modify this to return an invite code unique to campaign
    }
}
