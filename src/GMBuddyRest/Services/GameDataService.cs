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

namespace GMBuddyRest.Services
{
    public interface IGameDataService
    {
        /// <summary>
        /// Gets a list of campaigns for the currently signed-in user.
        /// </summary>
        /// <param name="gameType">The type of game being played, like DND35 or DND5</param>
        /// <returns>JArray containing an array of the user's Characters, each of which has an array of campaigns that they are in</returns>
        Task<JArray> GetCampaignsAsync(GameType gameType);

        /// <summary>
        /// Creates a new campaign with the given name and game master
        /// </summary>
        /// <param name="type">The type of game being played, like DND35 or DND5</param>
        /// <param name="name">The name of the campaign being created</param>
        /// <returns></returns>
        Task CreateCampaignAsync(GameType type, string name);
    }

    public enum GameType
    {
        DND35,
        DND5
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
        private readonly Dictionary<GameType, string> gameTypes;
        private readonly ILogger<GameDataService> logger;

        public GameDataService(IUserService userService, ILogger<GameDataService> logger)
        {
            httpClient = new HttpClient();
            this.userService = userService;
            this.logger = logger;
            this.gameTypes = new Dictionary<GameType, string>
            {
                [GameType.DND35] = "dnd35",
                [GameType.DND5] = "dnd5"
            };
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
        private string BuildURI(GameType gameType, string dataType, string actionType = "index", IDictionary<string, string> query = null)
        {
            string gtstring;

            // no need to check for the return value, because we know every GameType should have a corresponding string in the dictionary
            gameTypes.TryGetValue(gameType, out gtstring);

            var uri = new UriBuilder("http", "localhost", 5002, $"{gtstring}/{dataType}/{actionType}");

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

        public async Task<JArray> GetCampaignsAsync(GameType gameType)
        {
            var userEmail = await userService.GetUserAsync();
            var uri = BuildURI(gameType, "campaigns", null, new Dictionary<string, string>
            {
                ["email"] = userEmail
            });

            await httpClient.EnsureAuthenticated();
            var response = await httpClient.GetStringAsync(uri.ToString());

            return JArray.Parse(response);
        }

        public async Task CreateCampaignAsync(GameType type, string name)
        {
            var userEmail = await userService.GetUserAsync();
            var uri = BuildURI(type, "campaigns", null, null);

            await httpClient.EnsureAuthenticated();
            var reqBody = new StringContent(JObject.FromObject(new {
                GameMaster = userEmail,
                Name = name
            }).ToString(), Encoding.UTF8, "application/json");

            logger.LogWarning("Sending create campaign request to GameData API");
            var response = await httpClient.PostAsync(uri, reqBody);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("GameData API could not complete request");
                throw new Exception("Could not create campaign");
            }

            logger.LogInformation("Successfully created campaign with GameData API");
        }
    }
}
