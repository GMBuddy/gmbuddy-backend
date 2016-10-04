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
        public async Task<IActionResult> Index()
        {
            return new JsonResult(await games.GetCampaignsAsync(GameType.DND35));
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
                await games.CreateCampaignAsync(GameType.DND35, name);
                return Created("idkyet", null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
