using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;

namespace TimeTracking.WebApi.Controllers
{
    /// <summary>
    /// Milestone controller
    /// </summary>
    [ApiController]
    [Route("api/project")]
    public class ProjectController:ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Creates project by request
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ApiResponse<ProjectDto>> CreateProjectAsync([FromBody]ProjectDto dto)
        {
            return await _projectService.CreateProjectAsync(dto);
        }
        
        /// <summary>
        /// Returns paged response with all projects
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiPagedResponse<ProjectDetailsDto>> GetAll([FromQuery]PagedRequest request)
        {
            return await _projectService.GetAllProjectPagedAsync(request);
        }

        /// <summary>
        /// Returns project by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{projectId}")]
        public async Task<ApiResponse<ProjectDetailsDto>> GetMileStoneById([FromRoute] Guid projectId)
        {
            return await _projectService.GetProjectByIdAsync(projectId);
        }
    }
}