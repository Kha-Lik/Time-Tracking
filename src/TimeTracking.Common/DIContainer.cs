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
    public class DIContainer : IDIContainer
    {
        public DIContainer()
        {
            Services = new ServiceCollection();
        }

        public IServiceCollection Services { get; }

        public void AddService(ServiceDescriptor descriptor)
        {
            Services.Add(descriptor);
        }
    }
}
