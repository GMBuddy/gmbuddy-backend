using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Rest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Micro20.Controllers
{
    [Area("Micro20")]
    [Route("/[area]/[controller]")]
    public class CampaignsController : Controller
    {
        private readonly ILogger<CampaignsController> logger;
        private readonly GameService games;
        private readonly IUserService users;

        public CampaignsController(ILoggerFactory loggerFactory, GameService games, IUserService users)
        {
            logger = loggerFactory.CreateLogger<CampaignsController>();
            this.users = users;
            this.games = games;
        }

        [HttpGet("")]
        public async Task<IActionResult> ListCampaigns()
        {
            return Json(await games.ListCampaigns(users.GetUserId()));
        }

        [HttpGet("{campaignId}")]
        public async Task<IActionResult> GetCampaign(Guid campaignId)
        {
            try
            {
                var campaign = await games.GetCampaign(campaignId, users.GetUserId());
                return Json(campaign);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCampaign(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            try
            {
                var campaignId = await games.AddCampaign(name, users.GetUserId());
                return CreatedAtAction(nameof(GetCampaign), new { campaignId }, new { CampaignId = campaignId });
            }
            catch (DataNotCreatedException e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
