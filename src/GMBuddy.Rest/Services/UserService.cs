using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using GMBuddy.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GMBuddy.Rest.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Gets the user ID of the currently signed in user
        /// </summary>
        /// <exception cref="UnauthorizedException"></exception>
        /// <returns></returns>
        string GetUserId();
    }

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor context;

        public UserService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public string GetUserId()
        {
            try
            {
                return context.HttpContext.User.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException)
            {
                throw new UnauthorizedException("User ID could not be retrieved", e);
            }
        }
    }
}
