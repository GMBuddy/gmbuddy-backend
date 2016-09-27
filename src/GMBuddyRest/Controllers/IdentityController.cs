using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GMBuddyRest.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        // GET: api/identity
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(User.Claims.Select((c) => new { c.Type, c.Value }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string username, string password)
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "GMBuddyRest", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, "GMBuddyApi");

            if (tokenResponse.IsError)
            {
                return BadRequest();
            }

            return new JsonResult(tokenResponse.Json);
        }
    }
}
