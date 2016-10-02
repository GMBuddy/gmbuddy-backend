using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GMBuddyRest.Services
{
    public interface IUserService
    {
        Task<string> GetUserAsync();
    }

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor context;

        public UserService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public async Task<string> GetUserAsync()
        {
            // try to get an existing name claim first
            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            if (claim != null)
            {
                return claim;
            }

            // Only hit the identity server if the user doesnt have a name claim already
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var authHeader = context.HttpContext.Request.Headers
                .FirstOrDefault(h => h.Key == "Authorization")
                .Value.ToString()
                .Split(' ');
            if (authHeader.Length != 2 && !authHeader[0].Equals("Bearer"))
            {
                return null;
            }
            
            var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint, authHeader[1]);
            var userInfo = await userInfoClient.GetAsync();

            return userInfo.IsError ? null : userInfo.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
        }
    }
}
