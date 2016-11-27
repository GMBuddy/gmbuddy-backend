using System;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20.GameService;
using GMBuddy.Games.Micro20.InputModels;
using GMBuddy.Games.Micro20.OutputModels;
using GMBuddy.Rest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
            catch (ArgumentNullException)
            {
                return Unauthorized();
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
                var sheet = await games.GetCharacter(characterId, userId);
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
        public async Task<IActionResult> ModifyCharacter(CharacterModification model, bool updateSockets = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = users.GetUserId();

            try
            {
                await games.ModifyCharacter(model, userId);

                if (updateSockets)
                {
                    // var sheet = games.GetCharacter(model.CharacterId.Value, userId);
                    // await sockets.Emit("campaignId", "character/FETCH", sheet);
                    logger.LogInformation("Updating sockets");
                }
                else
                {
                    logger.LogInformation("Not updating sockets");
                }

                return NoContent();
            }
            catch (UnauthorizedException)
            {
                logger.LogInformation($"User {userId} tried to modify {model.CharacterId} but was not authorized to do so");
                return Unauthorized();
            }
            catch (DataNotFoundException)
            {
                logger.LogInformation($"Could not modify non-existent character {model.CharacterId}");
                return NotFound();
            }
        }

        /// <summary>
        /// This is a workaround to allow CampaignId to be explicitly reassigned to null,
        /// which is not allowed with normal character modification
        /// </summary>
        [HttpPut("{CharacterId}/CampaignId")]
        public async Task<IActionResult> ModifyCharacterCampaign(Guid characterId, [FromBody] CharacterCampaignModification model, bool updateSockets = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = users.GetUserId();

            try
            {
                bool differentCampaign = false;
                if (updateSockets)
                {
                    var character = await games.GetCharacter(characterId, userId);
                }

                bool changed = await games.ModifyCharacterCampaign(characterId, model, userId);
                if (updateSockets && changed)
                {
                    await sockets.Leave();
                }
            }
            catch (UnauthorizedException)
            {
                logger.LogInformation($"User {userId} tried to modify {characterId} but was not authorized to do so");
                return Unauthorized();
            }
            catch (DataNotFoundException)
            {
                logger.LogInformation($"Could not modify non-existent character {characterId}");
                return NotFound();
            }

            return NoContent();
        }
    }
}
