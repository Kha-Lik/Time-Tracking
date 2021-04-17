using System;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Dal.Impl;
using TimeTracking.Tests.Common;

namespace TimeTracking.UnitTests
{
    internal static class DbOptionsFactory
    {
        public static DbContextOptions<TimeTrackingDbContext> GetTimeTrackingDbOptions()
        {
            var options = new DbContextOptionsBuilder<TimeTrackingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new TimeTrackingDbContext(options);
            SeedHelper.SeedData(context);
            return options;
        }
    }
}