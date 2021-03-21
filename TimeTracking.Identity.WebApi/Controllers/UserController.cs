using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.Models.Dtos;

namespace TimeTracking.Identity.WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/user")]
    public class UserController:ControllerBase
    {
        private readonly IUserIdentityService _userIdentityService;

        public UserController(IUserIdentityService userIdentityService)
        {
            _userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get all users 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-users")]
        public  async Task<ApiResponse<List<UserDto>>> GetAll()
        {
           return  await  _userIdentityService.GetAllUsers();
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<ApiResponse<UserDto>> Get(Guid userId)
        {
            return await _userIdentityService.GetUsersById(userId);
        }
    }
}