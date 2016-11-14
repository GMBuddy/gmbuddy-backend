using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20.GameService
{
    public partial class GameService
    {
        private readonly DbContextOptions options;

        public GameService(DbContextOptions options = null)
        {
            this.options = options ?? new DbContextOptionsBuilder().Options;
        }
    }
}
