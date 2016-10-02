using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyData.Data.DND35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GMBuddyData.Controllers.DND35
{
    [Area("DND35")]
    public class CampaignsController : Controller
    {
        private GameContext db;

        public CampaignsController(GameContext db)
        {
            this.db = db;
        }

        // GET: dnd35/campaigns/list
        [HttpGet]
        public async Task<IActionResult> List(string email = null)
        {
            var campaigns = await db.Campaigns
                .Include(campaign => campaign.CampaignCharacters)
                .ThenInclude(cc => cc.Character)
                .ToListAsync();

            var filtered = email == null
                ? campaigns
                : campaigns.Where(c => c.CampaignCharacters.Any(cc => cc.Character.UserEmail == email));

            return Json(filtered.Select(c => new
            {
                c.Name,
                Characters = c.CampaignCharacters.Select(cc => new
                {
                    cc.Character.Name,
                    cc.Character.Bio,
                    Email = cc.Character.UserEmail
                })
            }));
        }
    }
}
