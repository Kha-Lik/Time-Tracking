using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TimeTracking.Dal.Impl;
using TimeTracking.IntegrationTests.Helpers;
using TimeTracking.Tests.Common;

namespace TimeTracking.IntegrationTests
{ 
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ConfigureJwtOptions(services);
                 var descriptor = services.SingleOrDefault(
                     d => d.ServiceType ==
                          typeof(DbContextOptions<TimeTrackingDbContext>));

                 services.Remove(descriptor);

                 services.AddDbContext<TimeTrackingDbContext>(options =>
                 {
                     options.UseInMemoryDatabase("InMemoryDbForTesting");
                 });

                 var sp = services.BuildServiceProvider();

                 using (var scope = sp.CreateScope())
                 {
                     var scopedServices = scope.ServiceProvider;
                     var db = scopedServices.GetRequiredService<TimeTrackingDbContext>();
                     var logger = scopedServices
                         .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                     db.Database.EnsureCreated();

                     try
                     {
                         SeedHelper.SeedData(db);
                     }
                     catch (Exception ex)
                     {
                         logger.LogError(ex, "An error occurred seeding the " +
                                             "database with test messages. Error: {Message}", ex.Message);
                     }
                 }
            });

                 // SeedDatabase<TimeTrackingDbContext>(services, SeedHelper.SeedData);
  
        }
    
        private static ServiceProvider GetInMemoryServiceProvider()
        {
            return new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }
        protected virtual void ConfigureJwtOptions(IServiceCollection services)
        {
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer =true,
                    ValidIssuer = MockJwtTokens.Issuer,
                    ValidateAudience =false,
                    ValidAudience = MockJwtTokens.Audince,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = MockJwtTokens.SecurityKey,
                    RequireExpirationTime =true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }

        protected virtual void RemoveDbContext<TContext>(IServiceCollection services)
        where TContext:DbContext
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<TContext>));

         
                services.Remove(descriptor);
        }

        public virtual void ReSeedDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TimeTrackingDbContext>();
       
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            try
            {
               SeedHelper.SeedData(context);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        }
    }
}