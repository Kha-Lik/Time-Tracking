using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.WebApi.Controllers
{
    /// <summary>
    /// Role controller
    /// </summary>
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// Role controller constructor
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Add user to role
        /// </summary>
        /// <param name="request">AddToRoleRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-to-role")]
        public async Task<ApiResponse> AddToRole([FromBody] AddToRoleRequest request)
        {
            return await _roleService.AddUserToRoleAsync(request);
        }

    }
}
