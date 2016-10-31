using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20.Data
{
    public class Micro20DataContext : DbContext
    {
        public Micro20DataContext()
        {
        }

        public Micro20DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite($"Filename={Utils.GetDatabasePath("micro20")}");
            }
        }

        public DbSet<Micro20Campaign> Campaigns { get; set; }

        public DbSet<Character> Characters { get; set; }
    }
}
