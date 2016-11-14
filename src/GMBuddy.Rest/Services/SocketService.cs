using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        Task SendAsync(string campaignId, string action, object data);
    }

    public class SocketService : ISocketService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SocketService> logger;

        public SocketService(ILoggerFactory loggerFactory)
        {
            httpClient = new HttpClient();
            logger = loggerFactory.CreateLogger<SocketService>();
        }

        public async Task SendAsync(string campaignId, string action, object data)
        {
            string encodedAction = Uri.EscapeDataString(action);
            string encodedCampaign = Uri.EscapeDataString(campaignId);
            string encodedData = JsonConvert.SerializeObject(data);
            var uri = new Uri($"http://localhost:5002/{encodedCampaign}/{encodedAction}");
            var response = await httpClient.PostAsync(uri, new StringContent(encodedData, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"Successfully sent socket update for campaign {campaignId}");
            }
            else
            {
                logger.LogWarning($"Could not send socket update for campaign {campaignId}");
            }
        }
    }
}