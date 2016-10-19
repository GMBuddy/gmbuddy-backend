using System;
using GMBuddy.Games.Dnd35.Models;
using Microsoft.EntityFrameworkCore;

namespace GMBuddy.Games.Dnd35.Data
{
    internal class Dnd35DataContext : DbContext
    {
        public Dnd35DataContext()
        {
        }

        public Dnd35DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite($"Filename={Utils.GetDatabasePath("dnd35")}");
        }

        public DbSet<Dnd35Campaign> Campaigns { get; set; }
    }
}
