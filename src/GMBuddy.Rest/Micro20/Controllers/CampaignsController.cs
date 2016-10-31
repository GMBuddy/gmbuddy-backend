using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.InputModels;
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

        public CampaignsController(ILoggerFactory loggerFactory, GameService games)
        {
            logger = loggerFactory.CreateLogger<CampaignsController>();

            this.games = games;
        }

        [HttpGet("")]
        public async Task<IActionResult> ListCampaigns()
        {
            return Json(await games.GetCampaigns());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaign(string id)
        {
            var campaigns = await games.GetCampaigns();

            // TODO: Provide filter to games.GetCampaigns() to do this internally
            return Json(campaigns.Single(c => c.CampaignId.ToString().Equals(id)));
        }

        [HttpPost("{CampaignId}/Characters")]
        public async Task<IActionResult> JoinCampaign(NewCharacter model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await games.AddCharacter(model);
                return Created(string.Empty, new {CharacterId = result.ToString()});
            }
            catch (DataNotCreatedException e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCampaign(string name)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var campaignId = await games.AddCampaign(name, userId);

                return CreatedAtAction(nameof(GetCampaign), new { Id = campaignId }, new { CampaignId = campaignId });
            }
            catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
