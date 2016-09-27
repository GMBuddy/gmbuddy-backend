using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GMBuddyRest.Controllers
{
    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        private readonly DiscoveryClient disco = new DiscoveryClient("http://localhost:5000");

        // GET: /Authorization/Start
        [HttpGet]
        public async Task<IActionResult> Start()
        {
            var doc = await disco.GetAsync();
            var request = new AuthorizeRequest(doc.AuthorizationEndpoint);
            var url = request.CreateAuthorizeUrl(
                clientId:     "GMBuddyRest",
                responseType: OidcConstants.ResponseTypes.CodeIdToken,
                responseMode: OidcConstants.ResponseModes.FormPost,
                redirectUri:  "http://localhost:5001/Authorization/Callback",
                state:        CryptoRandom.CreateUniqueId(),
                nonce:        CryptoRandom.CreateUniqueId())
            ;

            return Redirect(url);
        }

        // POST: /Authorization/Callback
        // After the user signs in and grants permission to our application, they will be redirected back here
        // We will then redirect to a route client-side with an access token in a url hash, for the js app to store client-side
        
    }
}
