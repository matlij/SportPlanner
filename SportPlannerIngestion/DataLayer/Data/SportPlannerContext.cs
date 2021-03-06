﻿using Microsoft.EntityFrameworkCore;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerIngestion.DataLayer.Data
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
            optionsBuilder.EnableSensitiveDataLogging();
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
