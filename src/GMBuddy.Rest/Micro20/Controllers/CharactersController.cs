using System;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Rest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Micro20.Controllers
{
    [Area("Micro20")]
    [Route("[area]/[controller]")]
    public class CharactersController : Controller
    {
        private readonly GameService games;
        private readonly ILogger<CharactersController> logger;
        private readonly IUserService users;

        public CharactersController(GameService games, ILoggerFactory loggerFactory, IUserService users)
        {
            this.games = games;
            this.users = users;
            logger = loggerFactory.CreateLogger<CharactersController>();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCharacter(NewCharacter model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = users.GetUserId();
                var result = await games.CreateCharacter(model, userId);
                return Created(string.Empty, new { CharacterId = result.ToString() });
            }
            catch (DataNotCreatedException e)
            {
                logger.LogInformation("Could not create character");
                return BadRequest(new { Error = e.Message });
            }
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacter(Guid characterId)
        {
            string userId = users.GetUserId();

            try
            {
                var sheet = await games.GetSheet(characterId, userId);
                return Json(sheet);
            }
            catch (DataNotFoundException)
            {
                logger.LogInformation($"Could not find character {characterId}");
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                logger.LogInformation($"User {userId} tried to modify {characterId} but was not authorized to do so");
                return Unauthorized();
            }
        }

        [HttpPut("{CharacterId}")]
        public async Task<IActionResult> ModifyCharacter(CharacterModification model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = users.GetUserId();

            try
            {
                await games.ModifyCharacter(model, userId);
                return NoContent();
            }
            catch (UnauthorizedException)
            {
                logger.LogInformation($"User {userId} tried to modify {model.CharacterId} but was not authorized to do so");
                return Unauthorized();
            }
        }
    }
}
