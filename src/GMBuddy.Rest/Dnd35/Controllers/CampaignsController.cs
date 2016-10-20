using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Dnd35.Controllers
{
    [Area("Dnd35")]
    public class CampaignsController : Controller
    {
        private readonly ILogger<CampaignsController> logger;
        private readonly Dnd35GameService games;

        public CampaignsController(ILoggerFactory loggerFactory, Dnd35GameService games)
        {
            logger = loggerFactory.CreateLogger<CampaignsController>();

            this.games = games;
        }

        [HttpGet("/[area]/[controller]")]
        public async Task<IActionResult> ListCampaigns()
        {
            return Json(await games.GetCampaignsAsync());
        }

        [HttpGet("/[area]/[controller]/{id}")]
        public async Task<IActionResult> GetCampaign(string id)
        {
            var campaigns = await games.GetCampaignsAsync();

            // TODO: Provide filter to games.GetCampaignsAsync() to do this internally
            return Json(campaigns.Single(c => c.CampaignId.ToString().Equals(id)));
        }

        [HttpPost("/[area]/[controller]")]
        public async Task<IActionResult> AddCampaign(string name)
        {
            string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.Role)).Value;
            bool success = await games.AddCampaignAsync(name, userId);

            if (!success)
            {
                return BadRequest();
            }

            return Created("idkyet", null);
        }
    }
}
