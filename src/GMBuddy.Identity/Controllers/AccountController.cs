using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Identity.Models;
using GMBuddy.Identity.Models.ViewModels;
using GMBuddy.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GMBuddy.Identity.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly JwtService jwtService;

        public AccountController(UserManager<User> userManager, JwtService jwtService)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Login()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }

        [HttpPost("Register")]
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
                return BadRequest();
            }

            var token = jwtService.CreateAsync(user);

            return Json(token);
        }
    }
}
