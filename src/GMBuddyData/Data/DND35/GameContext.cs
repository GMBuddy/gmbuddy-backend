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
            // don't allow a single user to create multiple campaigns with the same name
            builder.Entity<Campaign>().HasAlternateKey(c => new {c.GmEmail, c.Name});

            // don't allow a single user to create multiple characters with the same name
            builder.Entity<Character>().HasAlternateKey(c => new {c.UserEmail, c.Name});

            // don't allow a character to be tied to the same campaign twice
            builder.Entity<CampaignCharacter>().HasAlternateKey(cc => new {cc.CampaignId, cc.CharacterId});
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CampaignCharacter> CampaignCharacters { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
