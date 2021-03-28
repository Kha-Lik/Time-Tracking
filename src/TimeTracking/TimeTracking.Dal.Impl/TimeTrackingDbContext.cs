using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl
{
    public class TimeTrackingDbContext : DbContext
    {
        public TimeTrackingDbContext(DbContextOptions<TimeTrackingDbContext> options)
        : base(options)
        {

        }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<TimeTrackingUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Issue>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Issue>()
                .HasMany(e => e.WorkLogs)
                .WithOne();
            modelBuilder.Entity<Issue>()
                .HasOne(e => e.Milestone)
                .WithOne()
                .HasForeignKey<Issue>(e => e.MilestoneId);
            modelBuilder.Entity<Issue>()
                .HasOne(e => e.Project)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Issue>()
                .HasOne(e => e.TimeTrackingUserAssigned)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Issue>()
                .HasOne(e => e.TimeTrackingUserReporter)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Issue>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Team>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Team>()
                .HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId);


            modelBuilder.Entity<Project>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Project>()
                .HasIndex(e => e.Name)
                .IsUnique(true);
            modelBuilder.Entity<Project>()
                .HasMany(e => e.Teams)
                .WithOne();
            modelBuilder.Entity<Project>()
                .HasMany(e => e.Issues)
                .WithOne();
            modelBuilder.Entity<Project>()
                .HasMany(e => e.Milestones)
                .WithOne();

            modelBuilder.Entity<Milestone>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Milestone>()
                .HasMany(e => e.Issues)
                .WithOne();
            modelBuilder.Entity<Milestone>()
                .HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId);
            modelBuilder.Entity<Milestone>()
                .HasOne(e => e.CreatedByUser)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Milestone>()
                .Property(e => e.State)
                .HasConversion<string>();

            modelBuilder.Entity<WorkLog>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<WorkLog>()
                .HasIndex(e => e.UserId);
            modelBuilder.Entity<WorkLog>()
                .HasOne(e => e.TimeTrackingUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<WorkLog>()
                .HasOne(e => e.Issue)
                .WithMany()
                .HasForeignKey(e => e.IssueId);
            modelBuilder.Entity<WorkLog>()
                .Property(e => e.ActivityType)
                .HasConversion<string>();

            modelBuilder.Entity<TimeTrackingUser>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<TimeTrackingUser>()
                .HasOne(e => e.Team)
                .WithMany();

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditableEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateAuditableEntities()
        {
            var added = ChangeTracker.Entries<IAuditable>().Where(E => E.State == EntityState.Added).ToList();

            added.ForEach(E =>
            {
                E.Property(x => x.CreatedAt).CurrentValue = DateTimeOffset.UtcNow;
                E.Property(x => x.CreatedAt).IsModified = true;
            });

            var modified = ChangeTracker.Entries<IAuditable>().Where(entry => entry.State == EntityState.Modified).ToList();

            modified.ForEach(entry =>
            {
                entry.Property(x => x.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
                entry.Property(x => x.UpdatedAt).IsModified = true;

                entry.Property(x => x.CreatedAt).CurrentValue = entry.Property(x => x.CreatedAt).OriginalValue;
                entry.Property(x => x.CreatedAt).IsModified = false;
            });

        }
    }
}