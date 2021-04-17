using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TimeTracking.Dal.Impl;
using TimeTracking.Tests.Common;
using TimeTracking.WebApi;

namespace TimeTracking.IntegrationTests
{
    public class TimeTrackingWebApplicationFactory : IntegrationTestsWebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                RemoveDbContext<TimeTrackingDbContext>(services);
                services.AddDbContext<TimeTrackingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();
                ReSeedDatabase(sp).Wait();
            });
        }

        public async Task ReSeedDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TimeTrackingDbContext>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TimeTrackingWebApplicationFactory>>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            try
            {
                SeedHelper.SeedData(context);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        }
    }
}