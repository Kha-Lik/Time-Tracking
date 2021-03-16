using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface IProjectService
    {
        Task<ApiResponse<ProjectDto>> CreateProjectAsync(ProjectDto dto);
        Task<ApiResponse<ProjectDetailsDto>> GetProjectByIdAsync(Guid projectId);
        Task<ApiPagedResponse<ProjectDetailsDto>> GetAllProjectPagedAsync(PagedRequest request);
    }
}