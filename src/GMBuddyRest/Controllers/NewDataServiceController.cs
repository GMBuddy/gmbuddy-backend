using System.Collections.Generic;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35;
using Microsoft.AspNetCore.Mvc;

namespace GMBuddyRest.Controllers
{
    public class NewDataServiceController : Controller
    {
        private readonly GameService games;

        public NewDataServiceController(GameService games)
        {
            this.games = games;
        }

        // GET: api/NewDataService
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await games.GetCampaigns());
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name, string email)
        {
            if (await games.AddCampaignAsync(name, email))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
