using Microsoft.EntityFrameworkCore;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerIngestion.DataLayer.Data
{
    public class SportPlannerContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }

        public SportPlannerContext(DbContextOptions<SportPlannerContext> options) : base(options)
        {
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
