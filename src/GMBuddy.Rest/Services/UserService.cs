using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GMBuddy.Rest.Services
{
    public interface IUserService
    {
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
            return context.HttpContext.User.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        }
    }
}
