using System;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TimeTracking.Common.ServiceDiscovery
{
    public static class ServiceDiscoveryExtensions
    {
        public static void AddConsulServices(this IServiceCollection services,IConfiguration configuration)
        {
            var serviceDiscoveryConfiguration = new ServiceConfiguration();
            configuration.Bind(nameof(ServiceConfiguration), serviceDiscoveryConfiguration);
            var consulClient = CreateConsulClient(serviceDiscoveryConfiguration);
            services.AddSingleton(serviceDiscoveryConfiguration);
            services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
            services.AddSingleton<IConsulClient, ConsulClient>(p => consulClient);
        }

        private static ConsulClient CreateConsulClient(ServiceConfiguration serviceConfig)
        {
            return new ConsulClient(config =>
            {
                config.Address = serviceConfig.ServiceDiscoveryAddress;
            });
        }
    }
}