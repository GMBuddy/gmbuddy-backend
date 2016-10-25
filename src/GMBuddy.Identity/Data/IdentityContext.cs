using Microsoft.EntityFrameworkCore;
using GMBuddy.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GMBuddy.Identity.Data
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext()
        {
        }

        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Filename={Utils.GetDatabasePath("identity")}");
        }
    }
}
