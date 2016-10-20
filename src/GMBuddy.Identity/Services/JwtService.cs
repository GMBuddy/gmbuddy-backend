using System;
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

        /// <summary>
        /// Creates a JwtToken containing an encoded Access Token and an expiration time.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">User must not be null</exception>
        public JwtToken Create(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            logger.LogInformation($"Issuing access token for user ${user.UserName}");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email), 
                new Claim(JwtRegisteredClaimNames.Jti, AuthorizationConstants.Jti)
            };

            var header = new JwtHeader(options.SigningCredentials);
            var payload = new JwtPayload(
                AuthorizationConstants.Issuer,
                AuthorizationConstants.Audience,
                claims,
                AuthorizationConstants.NotBefore,
                AuthorizationConstants.Expiration);
            var jwt = new JwtSecurityToken(header, payload);
            string encoded = new JwtSecurityTokenHandler().WriteToken(jwt);

            logger.LogInformation($"Access token generated: {encoded}");

            return new JwtToken
            {
                AccessToken = encoded,
                ExpiresIn = (int)AuthorizationConstants.ValidFor.TotalSeconds
            };
        }

        public string Verify(string encodedToken)
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
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
