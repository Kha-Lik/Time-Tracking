using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeTracking.Identity.BL.Impl.Services;
using TimeTracking.Identity.Dal.Impl;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.WebApi;
using TimeTracking.Tests.Common;
using TimeTrackingIdentity.IntegrationTests.Fakes;

namespace TimeTrackingIdentity.IntegrationTests
{
    public class IdentityWebApplicationFactory : IntegrationTestsWebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                RemoveDbContext<TimeTrackingIdentityDbContext>(services);
                services.AddTransient<IEmailHelperService, FakeEmailHelper>();
                services.AddDbContext<TimeTrackingIdentityDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IdentityDb");
                });

                var sp = services.BuildServiceProvider();
                ReSeedDatabase(sp).Wait();
            });
        }


        public override async Task ReSeedDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TimeTrackingIdentityDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IdentityWebApplicationFactory>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            try
            {
                await DbSeedHelper.SeedUsersAndRolesAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        }
    }
}