using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddyRest.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GMBuddyRest.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private readonly IGameDataService gameData;
        public IdentityController(IGameDataService gameData)
        {
            this.gameData = gameData;
        }

        // GET: api/identity
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await gameData.GetCampaignsAsync("dnd35"));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string username, string password)
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "GMBuddyRestTesting", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, "GMBuddyApi openid profile email");

            if (tokenResponse.IsError)
            {
                return BadRequest();
            }

            return new JsonResult(tokenResponse.Json);
        }
    }
}
