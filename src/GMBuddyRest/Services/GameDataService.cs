using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace GMBuddyRest.Services
{
    public interface IGameDataService
    {
        Task<JArray> GetCampaignsAsync(string gameType);
    }

    public class GameDataService : IGameDataService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor context;
        private readonly IUserService userService;

        public GameDataService(IHttpContextAccessor context, IUserService userService)
        {
            httpClient = new HttpClient();
            this.context = context;
            this.userService = userService;
        }

        public async Task<JArray> GetCampaignsAsync(string gameType)
        {
            var userEmail = await userService.GetUserAsync();
            var uri = new UriBuilder("http", "localhost", 5002, $"{gameType}/campaigns/list", $"?email={userEmail}");
            var response = await httpClient.GetStringAsync(uri.ToString());

            return JArray.Parse(response);
        }
    }
}
