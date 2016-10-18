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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var response = await games.GetCampaignsAsync(gameTypes, campaignId, characterId);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return NotFound(await response.Content.ReadAsStringAsync());
                }
            }
            catch (NotImplementedException)
            {
                return BadRequest(new { Error = "We do not support the request that you're trying to make. Bug Jack or Mason to fix this." });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Campaigns
        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var response = await games.CreateCampaignAsync("dnd35", name);
                return Created("idkyet", await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
