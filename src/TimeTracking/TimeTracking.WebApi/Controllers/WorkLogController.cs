#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Dal.Impl.Seeds.Data;
using TimeTracking.Models;
using TimeTracking.Models.Requests;

namespace TimeTracking.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/workLog")]
    public class WorkLogController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;

        public WorkLogController(IWorkLogService workLogService)
        {
            _workLogService = workLogService;
        }

        /// <summary>
        /// Creates work log by parameters
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ApiResponse<WorkLogDto>> CreateWorkLog([FromBody]WorkLogDto dto)
        {
            return await _workLogService.CreateWorkLog(dto);
        }    
        
        /// <summary>
        /// Get user activities by user's team
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-activities")]
        public  async Task<ApiResponse<UserActivityDto>> GetActivitiesForUser([FromQuery]ActivitiesRequest request)
        {
            return await _workLogService.GetAllActivitiesForUser(request);
        }
        
        /// <summary>
        /// Updates worklog 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ApiResponse<WorkLogDto>> UpdateWorkLog([FromBody]WorkLogUpdateRequest request)
        {
            return await _workLogService.UpdateWorkLog(request);
        }     
        
        /// <summary>
        /// Returns a worklog by id
        /// </summary>
        /// <param name="workLogId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{workLogId}")]
        public async Task<ApiResponse<WorkLogDto>> GetWorkLog([FromRoute]Guid workLogId)
        {
            return await _workLogService.GetWorkLog(workLogId);
        }        
        
        /// <summary>
        /// Updates a work log status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "ProjectManager,TeamLead")]
        [HttpPost]
        [Route("update-status")]
        public async Task<ApiResponse<WorkLogDto>> UpdateWorkLogStatus([FromBody]UpdateWorkLogStatusRequest request)
        {
            return await _workLogService.UpdateWorkLogStatus(request.WorkLogId,request.IsApproved,request.Description);
        }

        /// <summary>
        ///  Returns all work logs paged
        /// </summary>
        /// <param name="pagedRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiPagedResponse<WorkLogDetailsDto>> GetAllWorkLogsPaged([FromRoute]PagedRequest pagedRequest)
        {
            return await _workLogService.GetAllWorkLogsPaged(pagedRequest);
        }
        
        /// <summary>
        /// Deletes a work log by id
        /// </summary>
        /// <param name="workLogId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResponse> DeleteWorklog([FromRoute]Guid workLogId)
        {
            return await _workLogService.DeleteWorkLog(workLogId);
        }
    }
}
#nullable restore