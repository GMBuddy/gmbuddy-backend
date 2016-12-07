using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMBuddy.Games.Micro20.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Micro20.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite($"Filename={Utils.GetDatabasePath("micro20")}");
            }
        }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Spell> Spells { get; set; }
    }
}
