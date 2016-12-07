using System;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.GameService;
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
        private readonly ISocketService sockets;

        public CampaignsController(ILoggerFactory loggerFactory, GameService games, IUserService users, ISocketService sockets)
        {
            logger = loggerFactory.CreateLogger<CampaignsController>();
            this.users = users;
            this.games = games;
            this.sockets = sockets;
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

        [HttpPut("{campaignId}")]
        public async Task<IActionResult> ModifyCampaign(Guid campaignId, CampaignModification model, bool sendUpdate = true)
        {
            try
            {
                var campaign = await games.ModifyCampaign(campaignId, users.GetUserId(), model);

                if (sendUpdate)
                {
                    await sockets.SendCampaign(campaign);
                }

                return Json(campaign);
            }
            catch (DataNotFoundException e)
            {
                logger.LogInformation(0, e, "Campaign or characters could not be found");
                return NotFound();
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
                var campaign = await games.AddCampaign(name, users.GetUserId());
                return CreatedAtAction(nameof(GetCampaign), new {campaignId = campaign.CampaignId}, campaign);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new {Error = e.Message});
            }
            catch (DataNotCreatedException e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
