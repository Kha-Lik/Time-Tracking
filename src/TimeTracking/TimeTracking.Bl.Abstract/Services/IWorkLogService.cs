using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface IWorkLogService
    {
        Task<ApiResponse<WorkLogDto>> CreateWorkLog(WorkLogDto dto);
        Task<ApiResponse<UserActivityDto>> GetAllActivitiesForUser(ActivitiesRequest request);
        Task<ApiPagedResponse<WorkLogDetailsDto>> GetAllWorkLogsPaged(PagedRequest pagedRequest);
        Task<ApiResponse<WorkLogDto>> UpdateWorkLog(WorkLogDto workLogDto,Guid workLogId);
        Task<ApiResponse<WorkLogDto>> GetWorkLog(Guid workLogId);
        Task<ApiResponse<WorkLogDto>> UpdateWorkLogStatus(Guid workLogId, bool isApproved, string description = null);
    }
}