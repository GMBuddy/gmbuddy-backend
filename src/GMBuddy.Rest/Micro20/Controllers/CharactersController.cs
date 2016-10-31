using System;
using System.Threading.Tasks;
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
