using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTracking.Bl.Impl;
using TimeTracking.Bl.Impl.Consumers;
using TimeTracking.Bl.Impl.Validators;
using TimeTracking.Common.Email;
using TimeTracking.Common.FluentValidator;
using TimeTracking.Common.Jwt;
using TimeTracking.Common.RabbitMq;
using TimeTracking.Common.Swager;
using TimeTracking.Dal.Impl;
using TimeTracking.WebApi.Internal;


namespace TimeTracking.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    c.RegisterValidatorsFromAssemblyContaining<AssignIssueToUserRequestValidator>();
                });

            services.AddDalDependencies(Configuration);
            services.AddBlLogicServices();
            services.AddSwaggerConfiguration("Time tracking");
            services.AddFluentValidatorServices(Configuration);
            services.AddJwtAuthServices(Configuration);
            services.AddRabbitMqConfiguration(Configuration);
            services.AddFluentEmailServices(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CurrentCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("AllowedHosts").Value)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
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