using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace GMBuddyRest.Services
{
    public interface IGameDataService
    {
        /// <summary>
        /// Gets a list of campaigns for the currently signed-in user.
        /// </summary>
        /// <param name="gameType">Required: the name of the game module (like "dnd35", "dnd5", etc.)</param>
        /// <returns>JArray containing an array of the user's Characters, each of which has an array of campaigns that they are in</returns>
        Task<JArray> GetCampaignsAsync(string gameType);
    }

    public static class GameDataAuthorizationService
    {
        /// <summary>
        /// Ensures that the HTTP Client has an authorization header set.
        /// If not, it will request a new authorization token from the GMBuddyIdentity service for access to GMBuddyData,
        /// and will assign that token to the client's bearer authorization header.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>void</returns>
        public static async Task EnsureAuthenticated(this HttpClient client)
        {
            if (client.DefaultRequestHeaders.Authorization?.Parameter != null)
            {
                return;
            }

            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "GMBuddyApi", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("GMBuddyData");

            if (tokenResponse.IsError)
            {
                throw new Exception("Could not access game data server");
            }

            client.SetBearerToken(tokenResponse.AccessToken);
        }
    }

    public class GameDataService : IGameDataService
    {
        private readonly HttpClient httpClient;
        private readonly IUserService userService;

        public GameDataService(IUserService userService)
        {
            httpClient = new HttpClient();
            this.userService = userService;
        }
        
        public async Task<JArray> GetCampaignsAsync(string gameType)
        {
            var userEmail = await userService.GetUserAsync();
            var uri = new UriBuilder("http", "localhost", 5002, $"{gameType}/campaigns/list", $"?email={userEmail}");

            await httpClient.EnsureAuthenticated();
            var response = await httpClient.GetStringAsync(uri.ToString());

            return JArray.Parse(response);
        }

        
    }
}
