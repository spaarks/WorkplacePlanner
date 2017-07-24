using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkplacePlanner.Data.Entities;

namespace WorkplacePlanner.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMembership> TeamMemberships { get; set; }

        public DbSet<TeamManager> TeamManagers { get; set; }

        public DbSet<CalendarEntry> CalendarEntries { get; set; }

        public DbSet<UsageType> UsageTypes { get; set; }

        public DbSet<GlobalDefaultUsageType> GlobalDefaultUsageTypes { get; set; }

        public DbSet<TeamDefaultUsageType> TeamDefaultUsageTypes { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>()
                    .HasIndex(t => t.Name)
                    .IsUnique();

            builder.Entity<Setting>()
                    .HasIndex(t => t.Name)
                    .IsUnique();
        }

        public override int SaveChanges()
        {
            //if (EnvironmentDescriptor != null)
            //{
                foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy = entry.Entity.LastUpdatedBy = 1;
                    entry.Entity.CreatedDate = entry.Entity.LastUpdatedDate = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = 1;
                    entry.Entity.LastUpdatedDate = DateTime.UtcNow;
                }
            //}
            return base.SaveChanges();
        }

        public void EnsureUptoDate()
        {
            Database.Migrate();
        }
    }
}
