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
    public class SpellsController : Controller
    {
        private readonly GameService games;
        private readonly ILogger<SpellsController> logger;
        private readonly IUserService users;

        public SpellsController(GameService games, ILoggerFactory loggerFactory, IUserService users)
        {
            this.games = games;
            this.users = users;
            logger = loggerFactory.CreateLogger<SpellsController>();
        }

        [HttpGet("")]
        public async Task<IActionResult> ListSpells()
        {
            try
            {
                var result = await games.ListSpells();
                return Json(result);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateSpell(NewSpell model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await games.CreateSpell(model);
                return Created(string.Empty, new { SpellId = result.ToString() });
            }
            catch (DataNotCreatedException e)
            {
                logger.LogInformation("Could not create spell");
                return BadRequest(new { Error = e.Message });
            }
        }

        [HttpGet("{spellId}")]
        public async Task<IActionResult> GetItem(Guid spellId)
        {
            try
            {
                var spell = await games.GetSpell(spellId);
                return Json(spell);
            }
            catch (DataNotFoundException)
            {
                logger.LogInformation($"Could not find spell {spellId}");
                return NotFound();
            }
        }

        [HttpPut("{SpellId}")]
        public async Task<IActionResult> ModifySpell(SpellModification model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await games.ModifySpell(model);
            return NoContent();
        }
    }
}