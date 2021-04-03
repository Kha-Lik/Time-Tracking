using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracking.Common
{
    public class DIContainer
    {
        private IServiceCollection _services;

        public DIContainer()
        {
            _services = new ServiceCollection();
        }

        public void AddDependency<TInterface, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    _services.AddSingleton<TInterface, TImplementation>();
                    break;
                case ServiceLifetime.Scoped:
                    _services.AddScoped<TInterface, TImplementation>();
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient<TInterface, TImplementation>();
                    break;
            }
        }

        public void AddDbContext<TDbContext>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            _services.AddDbContext<TDbContext>(lifetime);
        }

        public void AddConfiguration<TOptions>(IConfiguration configuration)
            where TOptions : class
        {
            _services.Configure<TOptions>(configuration);
        }

        public void AddHttpClient<TInterface, TImplementation>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TInterface : class
            where TImplementation : class, TInterface 
        {
            _services.AddHttpClient<TInterface, TImplementation>();
        }

        public void AddMassTransitAction(Action<IServiceCollectionBusConfigurator> configure = null)
        {
            _services.AddMassTransit(configure);
        }

        public void PopulateTo(IServiceCollection services)
        {
            foreach (var service in _services)
            {
                services.Add(service);
            }
        }
    }
}
