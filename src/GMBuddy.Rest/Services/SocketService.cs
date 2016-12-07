using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.OutputModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GMBuddy.Rest.Services
{
    public interface ISocketService
    {
        /// <summary>
        /// Tries to send a character model to the socket room for the given campaign
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        Task SendCharacter(CharacterSheet sheet);

        /// <summary>
        /// Tries to send a campaign model to the socket room for that campaign
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        Task SendCampaign(CampaignView campaign);

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

        public const string UpdatedCharacter = "character/FETCH";
        public const string UpdatedCampaign = "campaign/FETCH";

        private class SocketData
        {
            private object Data { get; set; }
            private Guid CampaignId { get; set; }
            private string Action { get; set; }

            public SocketData(object data, Guid campaignId, string action)
            {
                Data = data;
                CampaignId = campaignId;
                Action = action;
            }
        }

        public SocketService(ILoggerFactory loggerFactory, IUserService userService)
        {
            httpClient = new HttpClient();
            logger = loggerFactory.CreateLogger<SocketService>();
            this.userService = userService;
        }

        public async Task SendCharacter(CharacterSheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentException("Character must not be null", nameof(sheet));
            }

            if (sheet.Details.CampaignId == null)
            {
                logger.LogInformation($"Skipping socket update for character {sheet.Details.CharacterId} with null campaign");
                return;
            }

            // ASP.NET Core automatically camelCases JSON responses, but we have to do so manually
            string encodedData = JsonConvert.SerializeObject(new SocketData(sheet, sheet.Details.CampaignId.Value, UpdatedCharacter), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var uri = new Uri("http://localhost:4000/emit");
            var content = new StringContent(encodedData, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsync(uri, content);
                LogResponse(response, nameof(SendCharacter));
            }
            catch (HttpRequestException)
            {
                logger.LogError("Exception thrown by HTTP request to socket server");
            }
        }

        public async Task SendCampaign(CampaignView campaign)
        {
            if (campaign == null)
            {
                throw new ArgumentException("Campaign must not be null", nameof(campaign));
            }

            // ASP.NET Core automatically camelCases JSON responses, but we have to do so manually
            string encodedData = JsonConvert.SerializeObject(new SocketData(campaign, campaign.CampaignId, UpdatedCampaign), new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var uri = new Uri("http://localhost:4000/emit");
            var content = new StringContent(encodedData, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsync(uri, content);
                LogResponse(response, nameof(SendCharacter));
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
                logger.LogWarning("Can not leave null campaign");
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