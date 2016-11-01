using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Identity.Models;
using GMBuddy.Identity.Models.ViewModels;
using GMBuddy.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly JwtService jwtService;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> userManager, JwtService jwtService, ILoggerFactory loggerFactory)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
            logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            bool valid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!valid)
            {
                logger.LogWarning($"Could not log {model.Email} in");
                return new UnauthorizedResult();
            }

            return Json(jwtService.Create(user));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                logger.LogWarning($"Could not create user {user.Email}");
                return BadRequest();
            }
            
            return Json(jwtService.Create(user));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Ping()
        {
            // This returns the pretty name of the key, if necessary
            var claims = User.Claims.Select(c => new
            {
                Key = c.Properties.FirstOrDefault().Value ?? c.Type,
                c.Value
            });

            return Json(claims);
        }
    }
}
