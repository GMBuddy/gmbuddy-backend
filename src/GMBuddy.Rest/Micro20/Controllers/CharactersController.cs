using System;
using System.Threading.Tasks;
using GMBuddy.Exceptions;
using GMBuddy.Games.Micro20;
using GMBuddy.Games.Micro20.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace GMBuddy.Rest.Micro20.Controllers
{
    [Area("Micro20")]
    [Route("[area]/[controller]")]
    public class CharactersController : Controller
    {
        private readonly GameService games;

        public CharactersController(GameService games)
        {
            this.games = games;
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
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
