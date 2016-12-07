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
    [Route("[area]/[controller]")]
    public class ItemsController : Controller
    {
        private readonly GameService games;
        private readonly ILogger<ItemsController> logger;
        private readonly IUserService users;

        public ItemsController(GameService games, ILoggerFactory loggerFactory, IUserService users)
        {
            this.games = games;
            this.users = users;
            logger = loggerFactory.CreateLogger<ItemsController>();
        }

        [HttpGet("")]
        public async Task<IActionResult> ListItems()
        {
            try
            {
                var result = await games.ListItems();
                return Json(result);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateItem(NewItem model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await games.CreateItem(model);
                return Created(string.Empty, new { ItemId = result.ToString() });
            }
            catch (DataNotCreatedException e)
            {
                logger.LogInformation("Could not create item");
                return BadRequest(new { Error = e.Message });
            }
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            try
            {
                var item = await games.GetItem(itemId);
                return Json(item);
            }
            catch (DataNotFoundException)
            {
                logger.LogInformation($"Could not find item {itemId}");
                return NotFound();
            }
        }

        [HttpPut("{ItemId}")]
        public async Task<IActionResult> ModifyItem(ItemModification model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await games.ModifyItem(model);
            return NoContent();
        }
    }
}
