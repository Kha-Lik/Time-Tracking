using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Seeds.Data
{
    public static  class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasData(ProjectsData.Projects);
            modelBuilder.Entity<Team>()
                .HasData(TeamsData.Teams);
            modelBuilder.Entity<TimeTrackingUser>()
                .HasData(TimeTrackingUsers.Users);
            modelBuilder.Entity<Milestone>()
                .HasData(MilestonesData.Milestones);
            modelBuilder.Entity<Issue>()
                .HasData(IssuesData.Issues);
        }
    }
}