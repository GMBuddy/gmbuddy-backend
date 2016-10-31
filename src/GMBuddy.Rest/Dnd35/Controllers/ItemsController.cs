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
    public class ItemsController : Controller
    {
        private readonly ILogger<CampaignsController> logger;
        private readonly Dnd35GameService games;

        public ItemsController(ILoggerFactory loggerFactory, Dnd35GameService games)
        {
            logger = loggerFactory.CreateLogger<ItemsController>();

            this.games = games;
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(string itemId)
        {
            var items = await games.GetItemsAsync();

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(items.Single(i => i.ItemId.ToString().Equals(itemId)));
        }

        //Should this just be called GetItems?
        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetItemsForCharacter(string characterId)
        {
            var items = await games.GetItemsAsync();
            var characterItems = items.Where(i => (i.CharacterId.ToString().Equals(characterId)));

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(characterItems);
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

        //TODO: Implement ModifyItemAttributes

        [HttpPost("")]
        public async Task<IActionResult> AssignItemToCharacter(string itemId, string characterId)
        {
            try
            {
                var character = await games.AssignItemToCharacterAsync(itemId,characterId); //not using userId because we want to allow GM to modify item list as well. How do we want to check that?
                return CreatedAtAction(nameof(GetCharacter), new {Id = character}, null); //Do we want to keep the concept of a CampaignCharacter and add CampaignCharacter to the database?
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
