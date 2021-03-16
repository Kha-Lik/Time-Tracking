using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using TimeTracking.Common;
using TimeTracking.Common.Email;
using TimeTracking.Common.FluentValidator;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Jwt;
using TimeTracking.Common.RabbitMq;
using TimeTracking.Common.Swager;
using TimeTracking.Identity.BL;
using TimeTracking.Identity.BL.Impl;
using TimeTracking.Identity.BL.Impl.Settings;
using TimeTracking.Identity.BL.Impl.Validators;
using TimeTracking.Identity.Dal;
using TimeTracking.Identity.Dal.Impl;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.WebApi.Internal;

namespace TimeTracking.Identity.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssemblyContaining<Startup>();
                    c.RegisterValidatorsFromAssemblyContaining<TokenExchangeRequestValidator>();
                });

            services.AddRabbitMqConfiguration(Configuration);
            services.AddJwtAuthServices(Configuration);
            services.AddBlLogicServices(Configuration);
            services.AddDalServices(Configuration);
            services.AddFluentValidatorServices(Configuration);
            services.AddFluentEmailServices(Configuration);
            services.AddSwaggerConfiguration("Time tracking identity api");
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CurrentCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("AllowedHosts").Value);
                        builder.WithMethods().AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("CurrentCorsPolicy");
        
            app.UseSwagger();

           // specifying the Swagger JSON endpoint.
           if (env.IsDevelopment())
           {
               app.UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web App V1");
               });
           }
          
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}