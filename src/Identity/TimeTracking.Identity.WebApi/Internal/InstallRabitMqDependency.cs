using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TimeTracking.Common.RabbitMq;

namespace TimeTracking.Identity.WebApi.Internal
{
    internal static class InatallRabitMqDependency
    {
        public static IServiceCollection AddRabbitMqConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitMqConfiguration = new RabbitMqConfiguration();
            configuration.Bind(nameof(RabbitMqConfiguration), rabbitMqConfiguration);
            services.AddSingleton(rabbitMqConfiguration);
            services.AddMassTransit(x =>
            {
                var host = rabbitMqConfiguration.Host;
                var userName = rabbitMqConfiguration.UserName;
                var password = rabbitMqConfiguration.Password;
                var port = Convert.ToUInt16(rabbitMqConfiguration.Port);

                //consumers registration
                
                x.AddBus(provider =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        // configure health checks for this bus instance
                        cfg.UseHealthCheck(provider);

                        cfg.Host(new Uri($"rabbitmq://{host}:{port}") ,host =>
                        {
                            host.Username(userName);
                            host.Password(password);
                        });

                        cfg.ReceiveEndpoint(rabbitMqConfiguration.Endpoint, ep =>
                        {
                            //consumers configuration
                        });
                    });

                    bus.Start();

                    return bus;
                });
            });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }
}