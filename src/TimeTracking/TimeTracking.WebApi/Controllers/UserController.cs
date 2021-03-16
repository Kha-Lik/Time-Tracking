using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;
using TimeTracking.Models.Requests;

namespace TimeTracking.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Add user to team by id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-to-team")]
        public async Task<ApiResponse<TimeTrackingUserDto>> AddUserToTeam([FromBody]AssignUserToTeamRequest request)
        {
            return await _userService.AddUserToTeam(request);
        }
        
        /// <summary>
        /// Get all users paged
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-users")]
        public  async Task<ApiPagedResponse<TimeTrackingUserDetailsDto>> GetAll([FromRoute]PagedRequest request)
        {
            return  await  _userService.GetAllUsers(request);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<ApiResponse<TimeTrackingUserDetailsDto>> Get([FromRoute]Guid userId)
        {
            return await _userService.GetUsersById(userId);
        }
    }
}