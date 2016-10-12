using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GMBuddyRest.Services
{
    public interface IGameDataService
    {
        /// <summary>
        /// Gets a list of campaigns for the currently signed-in user.
        /// </summary>
        /// <param name="gameTypes">The types of game being requested, like DND35 or DND5</param>
        /// <param name="campaignId">The id of a specific campaign</param>
        /// <param name="characterId">The id of a specific character</param>
        /// <returns>JArray containing an array of the user's Characters, each of which has an array of campaigns that they are in</returns>
        Task<string> GetCampaignsAsync(IEnumerable<string> gameTypes, string campaignId, string characterId);

        /// <summary>
        /// Creates a new campaign with the given name and game master
        /// </summary>
        /// <param name="type">The type of game being played, like DND35 or DND5</param>
        /// <param name="name">The name of the campaign being created</param>
        /// <returns></returns>
        Task<JObject> CreateCampaignAsync(string type, string name);
    }

    public interface derp
    {
        string merp { get; set; }
    }

    /// <summary>
    /// Adds HttpClient extension methods for proper authentication of GameData service requests
    /// </summary>
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

    /// <summary>
    /// Production implementation of the game data service. Takes care of authentication and issuing requests against the GameData API
    /// </summary>
    public class GameDataService : IGameDataService
    {
        private readonly HttpClient httpClient;
        private readonly IUserService userService;
        private readonly ILogger<GameDataService> logger;

        public GameDataService(IUserService userService, ILogger<GameDataService> logger)
        {
            this.userService = userService;
            this.logger = logger;

            httpClient = new HttpClient();
        }

        #region helpers

        /// <summary>
        /// Builds a GameData API URI from the specified parameters.
        /// </summary>
        /// <param name="gameType">The type of game whose resources are being requested</param>
        /// <param name="dataType">A string containing what type of data is being managed, like "campaigns" or "players"</param>
        /// <param name="actionType">Either a string containing the action on the given data type, or null, defaulting to "index"</param>
        /// <param name="query">A dictionary of key-value string pairs, representing query parameters. May be omitted.</param>
        /// <returns>A full, well-formed URI</returns>
        private static string BuildUri(string gameType, string dataType, string actionType = "index", IDictionary<string, string> query = null)
        {
            var uri = new UriBuilder("http", "localhost", 5002, $"{gameType}/{dataType}/{actionType}");

            // only build the query string if the query dictionary exists
            if (query != null)
            {
                var queryString = new StringBuilder("?");
                foreach (var kv in query) {
                    queryString.Append($"{kv.Key}={kv.Value}");

                    if (!kv.Equals(query.Last()))
                    {
                        queryString.Append("&");
                    }
                }

                uri.Query = queryString.ToString();
            }

            return uri.ToString();
        }
        #endregion

        public async Task<string> GetCampaignsAsync(
            IEnumerable<string> gameTypes,
            string campaignId,
            string characterId)
        {
            await httpClient.EnsureAuthenticated();
            var query = new Dictionary<string, string>();
            bool isSingleResponse = false;

            if (characterId != null)
            {
                query.Add("characterId", characterId);
            }
            else if (campaignId != null)
            {
                query.Add("campaignId", campaignId);
                isSingleResponse = true;
            }
            else
            {
                query.Add("email", await userService.GetUserAsync());
            }

            var campaigns = new List<derp>();

            foreach (var type in gameTypes)
            {
                var uri = BuildUri(type, "campaigns", null, query);

                var response = await httpClient.GetStringAsync(uri);

                if (isSingleResponse)
                {
                    campaigns.Add(JsonConvert.DeserializeObject<derp>(response));
                }
                else
                {
                    campaigns.AddRange(JsonConvert.DeserializeObject<IEnumerable<derp>>(response));
                }
            }

            return JsonConvert.SerializeObject(campaigns);
        }

        public async Task<JObject> CreateCampaignAsync(string type, string name)
        {
            var userEmail = await userService.GetUserAsync();
            var uri = BuildUri(type, "campaigns");

            await httpClient.EnsureAuthenticated();
            var reqBody = new StringContent(JObject.FromObject(new {
                GameMaster = userEmail,
                Name = name
            }).ToString(), Encoding.UTF8, "application/json");

            logger.LogInformation("Sending create campaign request to GameData API");
            var response = await httpClient.PostAsync(uri, reqBody);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("GameData API could not complete request");
                throw new Exception("Could not create campaign");
            }

            logger.LogInformation("Successfully created campaign with GameData API");

            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}
