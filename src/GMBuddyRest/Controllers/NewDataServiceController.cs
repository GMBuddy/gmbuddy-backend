using System;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35;
using Microsoft.AspNetCore.Mvc;

namespace GMBuddyRest.Controllers
{
    public class NewDataServiceController : Controller
    {
        private readonly Dnd35GameService dnd35;

        public NewDataServiceController(Dnd35GameService dnd35)
        {
            this.dnd35 = dnd35;
        }

        // GET: api/NewDataService
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await dnd35.GetCampaignsAsync());
        }

        [HttpPost]
        public IActionResult Post(string name, string email)
        {
            throw new NotImplementedException();
        }
    }
}
