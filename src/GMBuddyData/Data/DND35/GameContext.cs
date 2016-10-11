using GMBuddyData.Models.DND35;
using Microsoft.EntityFrameworkCore;

namespace GMBuddyData.Data.DND35
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Campaign>().HasAlternateKey(c => new { c.GmEmail, c.Name });
            builder.Entity<Character>().HasAlternateKey(c => new { c.UserEmail, c.Name });
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CampaignCharacter> CampaignCharacters { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
