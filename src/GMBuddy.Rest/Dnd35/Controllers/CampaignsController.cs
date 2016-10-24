using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Dnd35.Controllers
{
    [Area("Dnd35")]
    [Route("/[area]/[controller]")]
    public class CampaignsController : Controller
    {
        private readonly ILogger<CampaignsController> logger;
        private readonly Dnd35GameService games;

        public CampaignsController(ILoggerFactory loggerFactory, Dnd35GameService games)
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

        [HttpPost("")]
        public async Task<IActionResult> AddCampaign(string name)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var campaignId = await games.AddCampaignAsync(name, userId);

                return CreatedAtAction(nameof(GetCampaign), new {Id = campaignId}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        [HttpGet("{characterId, campaignId}")]
        public async Task<IActionResult> GetCharacter(string characterId, string campaignId)
        {
            var characters = await games.GetCharactersAsync();

            // TODO: Provide filter to games.GetCampaignsAsync() to do this internally
            return Json(characters.Single(c => (c.CharacterId.ToString().Equals(characterId)) && (c.CampaignId.ToString().Equals(campaignId))));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCharacter(string name, string bio)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var characterId = await games.AddCharacterAsync(userId, name, bio);

                return CreatedAtAction(nameof(GetCharacter), new {Id = characterId}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        //TODO: implement AddStatToCharacter, AssignCharacterToCampaign, GetItem, AddItem
        

        [HttpPost("")]
        public async Task<IActionResult> AddStatToCharacter(string statName, string statValue, string characterId, string campaignId)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var character = await games.AddStatToCharacterAsync(userId, statName, statValue, characterId, campaignId);

                return CreatedAtAction(nameof(GetCharacter), new {Id = character}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AssignCharacterToCampaign(string characterId, string campaignId)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var campaign = await games.AssignCharacterToCampaignAsync(userId,characterId,campaignId);
                return CreatedAtAction(nameof(GetCampaign), new {Id = campaign}, null); //Do we want to keep the concept of a CampaignCharacter and add CampaignCharacter to the database?
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        public async Task<IActionResult> GetItems(string characterId)
        {
            var items = await games.GetItemsAsync();

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(items);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(string itemId)
        {
            var items = await games.GetItemsAsync();

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(items.Single(i => i.ItemId.ToString().Equals(itemId)));
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetItemsForCharacter(string characterId, string campaignId)
        {
            var characters = await games.GetCharactersAsync();
            var character = characters.Single(c => (c.CharacterId.ToString().Equals(characterId)) && (c.CampaignId.ToString().Equals(campaignId)));
            var items = character.Items;

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(items);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddItem(string name, string description)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var itemId = await games.AddItemAsync(name, description);

                return CreatedAtAction(nameof(GetItem), new {Id = itemId}, null); //make this fail if item with same name exists
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
