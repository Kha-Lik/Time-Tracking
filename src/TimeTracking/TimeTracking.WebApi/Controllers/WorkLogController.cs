#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;
using TimeTracking.Models.Requests;

namespace TimeTracking.WebApi.Controllers
{
    [ApiController]
    [Route("api/workLog")]
    public class WorkLogController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;

        public WorkLogController(IWorkLogService workLogService)
        {
            _workLogService = workLogService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ApiResponse<WorkLogDto>> CreateWorkLog([FromBody]WorkLogDto dto)
        {
            return await _workLogService.CreateWorkLog(dto);
        }    
        
        [HttpGet]
        [Route("get-user-activities")]
        public  async Task<ApiResponse<UserActivityDto>> GetActivitiesForUser([FromQuery]ActivitiesRequest request)
        {
            return await _workLogService.GetAllActivitiesForUser(request);
        }
        
        [HttpPost]
        [Route("update")]
        public async Task<ApiResponse<WorkLogDto>> UpdateWorkLog([FromBody]WorkLogDto workLogDto,[FromRoute]Guid workLogId)
        {
            return await _workLogService.UpdateWorkLog(workLogDto, workLogId);
        }     
        
        [HttpGet]
        [Route("{workLogId}")]
        public async Task<ApiResponse<WorkLogDto>> GetWorkLog([FromRoute]Guid workLogId)
        {
            return await _workLogService.GetWorkLog(workLogId);
        }        
      
        [HttpPost]
        [Route("update-status")]
        public async Task<ApiResponse<WorkLogDto>> UpdateWorkLogStatus([FromBody]UpdateWorkLogStatusRequest request)
        {
            return await _workLogService.UpdateWorkLogStatus(request.WorkLogId,request.IsApproved);
        }

        [HttpGet]
        public async Task<ApiPagedResponse<WorkLogDetailsDto>> GetAllWorkLogsPaged(PagedRequest pagedRequest)
        {
            return await _workLogService.GetAllWorkLogsPaged(pagedRequest);
        }
    }
}
#nullable restore