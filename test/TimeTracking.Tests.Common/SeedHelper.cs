using System.Linq;
using TimeTracking.Dal.Impl;
using TimeTracking.Tests.Common.Data;

namespace TimeTracking.Tests.Common
{
    public class SeedHelper
    {
        public static void SeedData(TimeTrackingDbContext context)
        {
            context.Issues.AddRange(IssuesDbSet.Get().ToList());
            context.Projects.AddRange(ProjectsDbSet.Get().ToList());
            context.Milestones.AddRange(MilestonesDbSet.Get().ToList());
            context.Teams.AddRange(TeamsDbSet.Get().ToList());
            context.WorkLogs.AddRange(WorklogsDbSet.Get().ToList());
            context.Users.AddRange(UsersDbSet.Get().ToList());
            context.SaveChanges();
        }
    }
}