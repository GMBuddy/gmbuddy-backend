using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GMBuddyRest.Services;

namespace GMBuddyRest.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly IGameDataService games;
        public CampaignsController(IGameDataService games)
        {
            this.games = games;
        }

        // GET: api/Campaigns
        [HttpGet]
        public async Task<IActionResult> Index(
            IEnumerable<string> gameTypes,
            string campaignId,
            string characterId)
        {
            return new JsonResult(await games.GetCampaignsAsync(gameTypes, campaignId, characterId));
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await games.CreateCampaignAsync("dnd35", name);
                return Created("idkyet", null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
