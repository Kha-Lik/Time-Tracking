using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Common.Helpers;

namespace TimeTracking.Common.FluentValidator
{
    public static class FluentValidatorDIInstaller
    {
        public static IServiceCollection AddFluentValidatorServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            services.AddMvc(options =>
                {
                    options.Filters.Add(new ModelStateFilter());
                })
                .AddFluentValidation(options =>
                {
                  
                    options.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                    
                });
            return services;
        }
    }
}