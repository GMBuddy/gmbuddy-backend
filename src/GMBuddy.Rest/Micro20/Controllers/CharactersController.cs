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
    public class CharactersController : Controller
    {
        private readonly GameService games;
        private readonly ILogger<CharactersController> logger;
        private readonly IUserService users;
        private readonly ISocketService sockets;

        public CharactersController(GameService games, ILoggerFactory loggerFactory, IUserService users, ISocketService sockets)
        {
            this.games = games;
            this.users = users;
            this.sockets = sockets;
            logger = loggerFactory.CreateLogger<CharactersController>();
        }

        [HttpGet("")]
        public async Task<IActionResult> ListCharacters()
        {
            try
            {
                string userId = users.GetUserId();
                var result = await games.ListCharacters(userId);
                return Json(result);
            }
            catch (Exception e) when (e is ArgumentNullException || e is UnauthorizedException)
            {
                logger.LogInformation(0, e, "Could not list characters");
                return Forbid();
            }
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
                logger.LogWarning(0, e, "Could not create character");
                return BadRequest(new { Error = e.Message });
            }
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacter(Guid characterId)
        {
            string userId = users.GetUserId();

            try
            {
                var sheet = await games.GetCharacter(characterId, userId);
                return Json(sheet);
            }
            catch (DataNotFoundException e)
            {
                logger.LogInformation(0, e, $"Could not find character {characterId}");
                return NotFound();
            }
            catch (UnauthorizedException e)
            {
                logger.LogInformation(0, e, $"User {userId} tried to modify {characterId} but was not authorized to do so");
                return Forbid();
            }
        }

        [HttpPut("{characterId}")]
        public async Task<IActionResult> ModifyCharacter(Guid characterId, CharacterModification model, bool updateSockets = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = users.GetUserId();

            try
            {
                bool changed = await games.ModifyCharacter(characterId, model, userId);

                if (updateSockets && changed)
                {
                    var sheet = await games.GetCharacter(characterId, userId);
                    await sockets.SendCharacter(sheet);
                }

                return NoContent();
            }
            catch (UnauthorizedException e)
            {
                logger.LogInformation(0, e, $"User {userId} tried to modify {characterId} but was not authorized to do so");
                return Forbid();
            }
            catch (DataNotFoundException e)
            {
                logger.LogInformation(0, e, $"Could not modify non-existent character {characterId}");
                return NotFound();
            }
        }
    }
}
