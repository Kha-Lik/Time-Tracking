using Microsoft.AspNetCore.Hosting;
using TimeTracking.ReportGenerator.WebApi;
using TimeTracking.Tests.Common;

namespace ReportGenerator.IntegrationTests
{
    public class ReportGeneratorWebAppFactory : IntegrationTestsWebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }
    }
}