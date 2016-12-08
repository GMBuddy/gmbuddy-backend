using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StrongGrid;

namespace GMBuddy.Identity.Services
{
    public interface IEmailService
    {
        Task Send(string to, string name, string body);
    }

    public class EmailOptions
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly EmailOptions options;
        private readonly ILogger<EmailService> logger;
        private readonly Client client;

        public EmailService(ILoggerFactory loggerFactory, IOptions<EmailOptions> opts)
        {
            logger = loggerFactory.CreateLogger<EmailService>();
            options = opts.Value;
            client = new Client(options.ApiKey);
        }
        public async Task Send(string to, string name, string body)
        {
            const string apiKey = "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";

        }
    }
}
