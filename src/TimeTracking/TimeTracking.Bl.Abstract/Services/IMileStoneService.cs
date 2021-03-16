using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface IMileStoneService
    {
        Task<ApiResponse<MilestoneDetailsDto>> GetMileStoneById(Guid mileStoneId);
        Task<ApiPagedResponse<MilestoneDetailsDto>> GetAllMilestonesPaged(PagedRequest request);
        Task<ApiResponse<MilestoneDto>> CreateMileStoneAsync(MilestoneDto dto);
    }
}