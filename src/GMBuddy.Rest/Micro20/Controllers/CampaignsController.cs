using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Micro20.Controllers
{
    [Area("Micro20")]
    [Route("/[area]/[controller]")]
    public class CampaignsController : Controller
    {
        private readonly ILogger<CampaignsController> logger;
        private readonly Micro20GameService games;

        public CampaignsController(ILoggerFactory loggerFactory, Micro20GameService games)
        {
            logger = loggerFactory.CreateLogger<CampaignsController>();

            this.games = games;
        }

        [HttpGet("")]
        public async Task<IActionResult> ListCampaigns()
        {
            return Json(await games.GetCampaignsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaign(string id)
        {
            var campaigns = await games.GetCampaignsAsync();

            // TODO: Provide filter to games.GetCampaignsAsync() to do this internally
            return Json(campaigns.Single(c => c.CampaignId.ToString().Equals(id)));
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinCampaign(string characterId)
        {
            
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCampaign(string name)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var campaignId = await games.AddCampaignAsync(name, userId);

                return CreatedAtAction(nameof(GetCampaign), new { Id = campaignId }, new { CampaignId = campaignId });
            }
            catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
