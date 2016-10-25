using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace GMBuddy
{
    public static class AuthorizationConstants
    {
        /// <summary>
        /// TODO: Use a better secret
        /// </summary>
        public static SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TEMPORARYPASSWORDPLEASECHANGE"));

        /// <summary>
        /// "iss" (Issuer) Claim
        /// </summary>
        public static string Issuer => "GMBuddyIdentity";

        /// <summary>
        /// "aud" (Audience) Claim
        /// </summary>
        public static string Audience => "http://localhost:5000";

        /// <summary>
        /// "nbf" (Not Before) Claim (default is UTC NOW)
        /// </summary>
        public static DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// "iat" (Issued At) Claim (default is UTC NOW)
        /// </summary>
        public static DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// Set the timespan the token will be valid for (default is 5 days)
        /// </summary>
        public static TimeSpan ValidFor => TimeSpan.FromDays(5);

        /// <summary>
        /// "exp" (Expiration Time) Claim (returns IssuedAt + ValidFor)
        /// </summary>
        public static DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public static string Jti => Guid.NewGuid().ToString();
    }
}
