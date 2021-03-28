using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Dal.Impl.Seeds.Data;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Seeds
{
    public static class TimeTrackingDbContextSeed
    {
        public static void SeedTimeTrackingUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeTrackingUser>().HasData(DbSeedData.TimeTrackingUserData());
        }
    }

}