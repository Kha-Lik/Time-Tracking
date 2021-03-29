using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Common.HttpClientHandler;
using TimeTracking.Common.Jwt;
using TimeTracking.ReportGenerator.Bl.Abstract;
using TimeTracking.ReportGenerator.Bl.Impl.Services;
using TimeTracking.ReportGenerator.Bl.Impl.Settings;

namespace TimeTracking.ReportGenerator.Bl.Impl
{
    public static class BalDependencyInstaller
    {
        public static IServiceCollection AddBlLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            /*
            container.RegisterTransient<IReportService, ReportService>();
            container.RegisterTransient<IReportExporter, ReportExporter>();
            container.RegisterTransient<IWorkLogClientService, WorkLogClientService>();
            container.RegisterTransient<IHttpProvider, HttpClientProvider>();
            */
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportExporter, ReportExporter>();
            services.AddTransient<IWorkLogClientService, WorkLogClientService>();
            services.AddHttpClient<IHttpProvider, HttpClientProvider>();
            services.AddConfig(configuration);
            services.AddHttpContextAccessor();
            return services;
        }

        private static IServiceCollection AddConfig(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<TimeTrackingClientSettings>(configuration.GetSection(nameof(TimeTrackingClientSettings)));
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            return services;
        }
    }
}