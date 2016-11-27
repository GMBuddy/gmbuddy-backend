using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GMBuddy.Rest.Services
{
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
        private readonly IHttpContextAccessor httpContext;

        public SocketService(ILoggerFactory loggerFactory, IHttpContextAccessor context)
        {
            httpClient = new HttpClient();
            logger = loggerFactory.CreateLogger<SocketService>();
            httpContext = context;
        }

        public async Task Emit(string campaignId, string action, object data)
        {
            string encodedAction = Uri.EscapeDataString(action);
            string encodedCampaign = Uri.EscapeDataString(campaignId);
            string encodedData = JsonConvert.SerializeObject(data);
            var uri = new Uri($"http://localhost:4000/emit/{encodedCampaign}/{encodedAction}");
            var content = new StringContent(encodedData, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, content);

            LogResponse(response, nameof(Emit));
        }

        public async Task Leave(string campaignId)
        {
            string encodedCampaign = Uri.EscapeDataString(campaignId);
            var uri = new Uri($"http://localhost:4000/leave/{encodedCampaign}");
            var response = await httpClient.PostAsync(uri, null);

            LogResponse(response, nameof(Leave));
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