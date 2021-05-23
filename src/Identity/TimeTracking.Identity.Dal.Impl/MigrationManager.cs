using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTracking.Identity.Dal.Impl.Seed;

namespace TimeTracking.Identity.Dal.Impl
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<TimeTrackingIdentityDbContext>())
                {
                    appContext.Database.Migrate();
                }
            }
            return host;
        }
    }
}