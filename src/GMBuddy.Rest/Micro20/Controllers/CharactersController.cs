using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.InputModels;
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

        public CharactersController(GameService games, ILoggerFactory loggerFactory)
        {
            this.games = games;
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
                string userId = User.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
                var result = await games.CreateCharacter(model, userId);
                return Created(string.Empty, new { CharacterId = result.ToString() });
            }
            catch (DataNotCreatedException e)
            {
                logger.LogWarning("Could not create character");
                return BadRequest(new { Error = e.Message });
            }
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacter(Guid characterId)
        {
            try
            {
                var sheet = await games.GetSheet(characterId);
                return Json(sheet);
            }
            catch (DataNotFoundException)
            {
                logger.LogWarning($"Could not find character {characterId}");
                return NotFound();
            }
        }

        [HttpPut("{CharacterId}")]
        public async Task<IActionResult> ModifyCharacter(CharacterModification model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await games.ModifyCharacter(model);
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogWarning($"Could not modify character {model.CharacterId}");
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
