using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Bl.Impl.Helpers;
using TimeTracking.Bl.Impl.Mappers;
using TimeTracking.Bl.Impl.Services;
using TimeTracking.Bl.Impl.Validators;
using TimeTracking.Common.Mappers;
using TimeTracking.Common.Pagination;
using TimeTracking.Common.Wrapper;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl
{
    public static class BLDependencyInjector
    {
        public static IServiceCollection AddBlLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IIssueService, IssueService>();
            services.AddTransient<IMileStoneService, MileStoneService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWorkLogService, WorkLogService>();
            services.AddTransient<ISystemClock, SystemClock>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IEmailHelper, EmailHelper>();
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMappers();
            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddTransient<IModelMapper<Issue, IssueDetailsDto>, IssueDetailsMapper>();
            services.AddTransient<IBaseMapper<Issue,IssueDto>, IssueMapper>();
            
            services.AddTransient<IBaseMapper<Milestone,MilestoneDto>, MileStoneMapper>();
            services.AddTransient<IModelMapper<Milestone,MilestoneDetailsDto>,MilestoneDetailsMapper>();
            
            services.AddTransient<IBaseMapper<Project,ProjectDto>, ProjectMapper>();
            services.AddTransient<IModelMapper<Project,ProjectDetailsDto>, ProjectDetailsMapper>();
            
            services.AddTransient<IBaseMapper<Team,TeamDto>, TeamMapper>();
            services.AddTransient<IModelMapper<Team,TeamDetailsDto>, TeamDetailsMapper>();
            
            services.AddTransient<IBaseMapper<TimeTrackingUser,TimeTrackingUserDto>, TimeTrackingMapper>();
            services.AddTransient<IModelMapper<TimeTrackingUser,TimeTrackingUserDetailsDto>,TimeTrackingUserDtoMapper>();

            services.AddTransient<IBaseMapper<WorkLog,WorkLogDto>, WorkLogMapper>();
            services.AddTransient<IModelMapper<WorkLog,WorkLogDetailsDto>,WorkLogDetailsMapper>();
            
            return services;
        }
        
    }
}