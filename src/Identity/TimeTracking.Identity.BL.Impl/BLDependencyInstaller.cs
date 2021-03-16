using System.IO.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Common.Jwt;
using TimeTracking.Common.Mappers;
using TimeTracking.Identity.BL.Abstract.Factories;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.BL.Abstract.Validators;
using TimeTracking.Identity.BL.Impl.Factories;
using TimeTracking.Identity.BL.Impl.Mappers;
using TimeTracking.Identity.BL.Impl.Services;
using TimeTracking.Identity.BL.Impl.Settings;
using TimeTracking.Identity.BL.Impl.Validators;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Dtos;

namespace TimeTracking.Identity.BL.Impl
{
    public static class BLDependencyInstaller
    {
        public static IServiceCollection AddBlLogicServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
            services.AddTransient<IUserIdentityService, UserIdentityService>();
            services.AddTransient<IEmailHelperService, EmailHelperService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISystemClock, SystemClock>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddConfig(configuration);
            services.AddMappers();
            return services;
        }

        private static IServiceCollection AddConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ClientSenderSettings>(configuration.GetSection(nameof(ClientSenderSettings)));
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            return services;
        }   
        
        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddTransient<IBaseMapper<User, UserDto>,UserDtoMapper>();
            return services;
        }
    }
}