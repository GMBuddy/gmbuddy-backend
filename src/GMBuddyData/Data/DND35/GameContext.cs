using GMBuddyData.Models.DND35;
using Microsoft.EntityFrameworkCore;

namespace GMBuddyData.Data.DND35
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
            
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CampaignCharacter> CampaignCharacters { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
