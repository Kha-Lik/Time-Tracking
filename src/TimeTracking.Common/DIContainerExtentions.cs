using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracking.Common
{
    public static class DIContainerExtentions
    {
        public static void AddDependency<TInterface, TImplementation>(this IDIContainer container, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            var service = new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime);
            container.AddService(service);
        }

        public static void Populate(this IDIContainer container, IServiceCollection services)
        {
            foreach (var service in services)
            {
                container.AddService(service);
            }
        }
    }
}
