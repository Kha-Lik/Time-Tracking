using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TimeTracking.Dal.Impl.Seeds.Data
{
    public static class DbSeeder
    {
        public static void SeedData(TimeTrackingDbContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT  OFF");
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(ProjectsData.Projects);
               context.SaveChanges(); 
            }
            if (!context.Teams.Any())
            {
                context.Teams.AddRange(TeamsData.Teams);
                context.SaveChanges();
            }
     
            if (!context.Users.Any())
            {
                context.Users.AddRange(TimeTrackingUsers.Users);
               context.SaveChanges();
            }
            
            if (!context.Milestones.Any())
            {
                context.Milestones.AddRange(MilestonesData.Milestones);
               context.SaveChanges();
            }
            if (!context.Issues.Any())
            {
                context.Issues.AddRange(IssuesData.Issues);
                context.SaveChanges();
            }
            if (!context.WorkLogs.Any())
            { 
                context.WorkLogs.AddRange(WorkLogsData.WorkLogs);
                context.SaveChanges();
            }
            context.SaveChanges();
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[User] ON");
            transaction.Commit();
            }
        }
    }
}