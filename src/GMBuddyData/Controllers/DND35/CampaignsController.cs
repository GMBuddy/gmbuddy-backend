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

        // GET: dnd35/campaigns/list/{user}
        [HttpGet]
        public async Task<IActionResult> List(string email)
        {

            var characters = await db.Characters
                .Include((character) => character.CampaignCharacters)
                .ThenInclude((cc) => cc.Campaign)
                .Where((character) => character.UserEmail == email)
                .ToListAsync();

            return Json(characters.Select((c) => new
            {
                Name = c.Name,
                Bio = c.Bio,
                Campaigns = c.CampaignCharacters.Select((cc) => new
                {
                    Name = cc.Campaign.Name
                })
            }));
        }
    }
}
