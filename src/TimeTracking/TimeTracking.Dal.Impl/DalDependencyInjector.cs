using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Dal.Impl.Repositories;

namespace TimeTracking.Dal.Impl
{
    public static class DalDependencyInjector
    {
        public static IServiceCollection AddDalDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TimeTrackingDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TimeTrackingDbContext).Assembly.FullName)));

            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IWorklogRepository, WorklogRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IIssueRepository, IssueRepository>();
            services.AddTransient<IMilestoneRepository, MilestoneRepository>();
            return services;
        }
    }
}