using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTracking.Dal.Impl.Seeds.Data;

namespace TimeTracking.Dal.Impl
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<TimeTrackingDbContext>())
                {
                   appContext.Database.Migrate();
                  DbSeeder.SeedData(appContext);
                }
            }
            return host;
        }
    }
}