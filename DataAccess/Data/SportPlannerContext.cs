using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class SportPlannerContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }

        public SportPlannerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EventUser>()
                .HasKey(e => new { e.EventId, e.UserId });

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Identifier)
                .IsUnique();
        }
    }
}
