using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Impl.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<RoleService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<ApiResponse> AddUserToRoleAsync(AddToRoleRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                return new ApiResponse(
                    new ApiError()
                    {
                        ErrorCode = ErrorCode.UserNotFound,
                        ErrorMessage = $"No Accounts Registered with id {model.UserId}.",
                    });
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return new ApiResponse(
                    new ApiError()
                    {
                        ErrorCode = ErrorCode.RoleNotFound,
                        ErrorMessage = $"Role {model.RoleName} not found.",
                    });
            }

            var addUserResponse = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!addUserResponse.Succeeded)
            {
                _logger.LogWarning("Failed to add user {0} to role by reason {1}", model.UserId, addUserResponse.Errors);
                return new ApiResponse(
                    new ApiError()
                    {
                        ErrorCode = ErrorCode.AddToRoleFailed,
                        ErrorMessage = $"Add user {model.UserId} to role {model.RoleName} failed.",
                    });
            }

            return ApiResponse.Success();
        }
    }
}