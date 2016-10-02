using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddyIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace GMBuddyIdentity
{
    public class DataInitializer
    {
        public static async Task Init(UserManager<ApplicationUser> userManager)
        {
            var username = "testing@user.com";
            
            var account = await userManager.FindByEmailAsync(username);
            if (account == null)
            {
                var user = new ApplicationUser
                {
                    Email = username,
                    UserName = username
                };
                await userManager.CreateAsync(user, "Testing_123");
            }
        }
    }
}
