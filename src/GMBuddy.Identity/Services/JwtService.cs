using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GMBuddy.Identity.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GMBuddy.Identity.Services
{
    public class JwtService
    {
        private readonly ILogger<JwtService> logger;
        private readonly JwtOptions options;

        public JwtService(ILoggerFactory loggerFactory, IOptions<JwtOptions> options)
        {
            logger = loggerFactory.CreateLogger<JwtService>();
            this.options = options.Value;
        }

        public JwtToken CreateAsync(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, options.Jti)
            };

            var header = new JwtHeader(options.SigningCredentials);
            var payload = new JwtPayload(options.Issuer, options.Audience, claims, options.NotBefore, options.Expiration);
            var jwt = new JwtSecurityToken(header, payload);
            string encoded = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtToken
            {
                AccessToken = encoded,
                ExpiresIn = (int) options.ValidFor.TotalSeconds
            };
        }

        public string VerifyAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class JwtToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// See https://goblincoding.com/2016/07/03/issuing-and-authenticating-jwt-tokens-in-asp-net-core-webapi-part-i/ for more on this
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// "iss" (Issuer) Claim
        /// </summary>
        public string Issuer { get; set; } = "GMBuddyIdentity";

        /// <summary>
        /// "sub" (Subject) Claim
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// "aud" (Audience) Claim
        /// </summary>
        public string Audience { get; set; } = "http://localhost:5000";

        /// <summary>
        /// "nbf" (Not Before) Claim (default is UTC NOW)
        /// </summary>
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// "iat" (Issued At) Claim (default is UTC NOW)
        /// </summary>
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Set the timespan the token will be valid for (default is 5 days)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(5);

        /// <summary>
        /// "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public string Jti => Guid.NewGuid().ToString();

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
