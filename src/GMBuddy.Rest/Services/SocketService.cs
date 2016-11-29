using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GMBuddy.Rest.Services
{
    public static class SocketActions
    {
        public static string UpdatedCharacter = "character/FETCH";

        public static string UpdatedCampaign = "campaign/FETCH";
    }

    public interface ISocketService
    {
        /// <summary>
        /// Tries to send a message to the socket server, notifying it of new data for a given room
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Emit(string campaignId, string action, object data);

        /// <summary>
        /// Tries to send a message to the socket server notifying it that the current user has left a campaign
        /// </summary>
        Task Leave(string campaignId);
    }

    public class SocketService : ISocketService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SocketService> logger;
        private readonly IUserService userService;

        public SocketService(ILoggerFactory loggerFactory, IUserService userService)
        {
            httpClient = new HttpClient();
            logger = loggerFactory.CreateLogger<SocketService>();
            this.userService = userService;
        }

        public async Task Emit(string campaignId, string action, object data)
        {
            if (string.IsNullOrEmpty(campaignId) || string.IsNullOrEmpty(action))
            {
                logger.LogWarning("Can not emit message for null campaign or action");
                return;
            }

            // ASP.NET Core automatically camelCases JSON responses, but we have to do so manually
            string encodedData = JsonConvert.SerializeObject(new
            {
                Data = data,
                CampaignId = campaignId,
                Action = action
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var uri = new Uri("http://localhost:4000/emit");
            var content = new StringContent(encodedData, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsync(uri, content);
                LogResponse(response, nameof(Emit));
            }
            catch (HttpRequestException)
            {
                logger.LogError("Exception thrown by HTTP request to socket server");
            }
        }

        public async Task Leave(string campaignId)
        {
            if (string.IsNullOrEmpty(campaignId))
            {
                logger.LogWarning("Can not join null campaign");
                return;
            }

            // ASP.NET Core automatically camelCases JSON responses, but we have to do so manually
            string userId = userService.GetUserId();
            string encodedData = JsonConvert.SerializeObject(new
            {
                CampaignId = campaignId,
                UserId = userId
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var uri = new Uri("http://localhost:4000/leave");
            var content = new StringContent(encodedData, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsync(uri, content);
                LogResponse(response, nameof(Leave));
            }
            catch (HttpRequestException)
            {
                logger.LogError("Exception thrown by HTTP request to socket server");
            }
        }

        private void LogResponse(HttpResponseMessage response, string action)
        {
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"Successfully made {action.ToLower()} request to socket server");
            }
            else
            {
                logger.LogWarning($"Could not make {action.ToLower()} request to socket server");
                logger.LogWarning($"Socket service responded with {response.StatusCode}");
            }
        }
    }
}