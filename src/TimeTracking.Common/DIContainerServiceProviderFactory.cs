using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracking.Common
{
    public class DIContainerServiceProviderFactory : IServiceProviderFactory<DIContainer>
    {
        public DIContainer CreateBuilder(IServiceCollection services)
        {
            var container = new DIContainer();
            container.Populate(services);
            return container;
        }

        public IServiceProvider CreateServiceProvider(DIContainer containerBuilder)
        {
            if (containerBuilder == null)
                throw new ArgumentNullException(nameof(containerBuilder));

            return containerBuilder.Services.BuildServiceProvider();
        }
    }
}
