#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace TimeTracking.Common.Swager
{
    public static  class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, string title)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            //resolve all fluent validators
            var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType.GetInterfaces().Contains(typeof(IValidator))).ToList();
            serviceDescriptors.ForEach(descriptor => services.Add(ServiceDescriptor.Transient(typeof(IValidator), descriptor.ImplementationType)));
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1",
                    new OpenApiInfo { Title = title, 
                    Version = "v1" });
                swaggerGenOptions.AddFluentValidationRules();
                // The name of this security definition is linked to the Id below
                swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                swaggerGenOptions.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
                if (AppDomain.CurrentDomain.BaseDirectory != null)
                {
                    var commentsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
                    if (!File.Exists(commentsFile))
                    {
                        throw new FileNotFoundException($" Xml comments file does not exist! ({commentsFile})");
                    }
                    swaggerGenOptions.IncludeXmlComments(commentsFile);
                }

                swaggerGenOptions.UseInlineDefinitionsForEnums();
            });

            services.AddLogging(builder => builder.AddConsole().AddFilter(level => true));
            return services;
        }
    }
}
#nullable restore