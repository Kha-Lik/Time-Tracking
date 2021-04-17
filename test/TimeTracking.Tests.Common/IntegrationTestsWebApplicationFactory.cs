using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TimeTracking.Dal.Impl;
using TimeTracking.Tests.Common.Helpers;

namespace TimeTracking.Tests.Common
{
    public abstract class IntegrationTestsWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureJwtOptions);

        }

        protected virtual void ConfigureJwtOptions(IServiceCollection services)
        {
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = MockJwtTokens.Issuer,
                    ValidateAudience = false,
                    ValidAudience = MockJwtTokens.Audince,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = MockJwtTokens.SecurityKey,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }
        public string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            return MockJwtTokens.GenerateJwtToken(claims);
        }

        protected virtual void RemoveDbContext<TContext>(IServiceCollection services)
            where TContext : DbContext
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<TContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        public virtual Task ReSeedDatabase(IServiceProvider services)
        {
            return Task.CompletedTask;
        }
    }
}