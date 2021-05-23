using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Identity.Models.Responses;

namespace TimeTracking.Identity.WebApi.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserIdentityService _userIdentityService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Auth controller constructor
        /// </summary>
        /// <param name="userIdentityService"></param>
        /// <param name="tokenService"></param>
        public AuthController(IUserIdentityService userIdentityService,
            ITokenService tokenService)
        {
            _userIdentityService = userIdentityService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Register user account
        /// </summary>
        /// <param name="request">RegistrationRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<ApiResponse> Register([FromBody] RegistrationRequest request)
        {
            return await _userIdentityService.RegisterAsync(request);
        }

        /// <summary>
        /// Confirms a user email address
        /// </summary>
        /// <param name="request">EmailConfirmationRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirm-email")]
        public async Task<ApiResponse> ConfirmEmail([FromBody] EmailConfirmationRequest request)
        {
            if (request == null)
            {
                return ApiResponse.Failed();
            }
            return await _userIdentityService.ConfirmEmailAsync(request);
        }

        /// <summary>
        /// Token exchange request
        /// </summary>
        /// <param name="request">TokenExchangeRequest</param>
        /// <returns>Returns jwt token, refresh token and expiration time</returns>
        [HttpPost]
        [Route("token")]
        public async Task<AuthResponse> CreateToken([FromBody] TokenExchangeRequest request)
        {
            return await _tokenService.LoginAsync(request);
        }

        /// <summary>
        /// Refresh token 
        /// </summary>
        /// <param name="request">RefreshTokenRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<AuthResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return await _tokenService.RefreshTokenAsync(request.RefreshToken);
        }

        /// <summary>
        /// Forgot email sends request and send  an email with a link containing reset token
        /// </summary>
        /// <param name="request">ForgotPasswordRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        public async Task<ApiResponse> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            return await _userIdentityService.ForgotPasswordAsync(request);
        }

        /// <summary>
        /// Reset account password with reset token
        /// </summary>
        /// <param name="request">ResetPasswordRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        public async Task<ApiResponse> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            return await _userIdentityService.ResetPasswordAsync(request);
        }

        /// <summary>
        /// Resend email verification email with token link
        /// </summary>
        /// <param name="request">ResendEmailRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("resend-email-verification-code")]
        public async Task<ApiResponse> ResendVerificationEmail([FromBody] ResendEmailRequest request)
        {
            return await _userIdentityService.ResentEmailAsync(request);
        }

        /// <summary>
        /// Revokes sent token
        /// </summary>
        /// <param name="request">ResendEmailRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("revoke-token")]
        public async Task<AuthResponse> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            return await _tokenService.RevokeTokenAsync(request.Token);
        }
    }
}