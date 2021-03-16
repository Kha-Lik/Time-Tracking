using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;
using TimeTracking.Models.Requests;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface IUserService
    {
        Task<ApiResponse<TimeTrackingUserDto>> AddUserToTeam(AssignUserToTeamRequest request);
        Task<ApiResponse<TimeTrackingUserDetailsDto>> GetUsersById(Guid userId);
        Task<ApiPagedResponse<TimeTrackingUserDetailsDto>> GetAllUsers(PagedRequest request);
    }
}