using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.Models.Dtos;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Abstract.Services
{
    public interface IUserIdentityService
    {
        Task<ApiResponse> RegisterAsync(RegistrationRequest model);
        Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ApiResponse> ResentEmailAsync(ResendEmailRequest request);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse> ConfirmEmailAsync(EmailConfirmationRequest request);
        Task<ApiResponse<List<UserDto>>> GetAllUsers();
        Task<ApiResponse<UserDto>> GetUsersById(Guid userId);
    }
}