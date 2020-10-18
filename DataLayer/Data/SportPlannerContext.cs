using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public class SportPlannerContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:sportplannerserver.database.windows.net,1433;Initial Catalog=sportplannerdb;Persist Security Info=False;User ID=mattiaslij;Password=Njadoks189;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            //optionsBuilder.UseSqlServer("Server=localhost;Database=sportplannerdb;Trusted_Connection=True;");
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
