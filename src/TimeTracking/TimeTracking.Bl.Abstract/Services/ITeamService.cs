using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface ITeamService
    {
        Task<ApiResponse<TeamDto>> CreateTeamAsync(TeamDto dto);
        Task<ApiResponse<TeamDetailsDto>> GetTeamById(Guid teamId);
        Task<ApiPagedResponse<TeamDetailsDto>> GetAllTeamAsync(PagedRequest request);
    }
}