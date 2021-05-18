#nullable enable
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Email;
using TimeTracking.Common.Enums;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Impl.Settings;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Templates.Models;

namespace TimeTracking.Identity.BL.Impl.Services
{
    public interface IEmailHelperService
    {
        Task<ApiResponse> SendEmailConfirmationEmail(EmailSendRequest request);
        Task<ApiResponse> SendResetPasswordConfirmationEmail(EmailSendRequest request);
    }

    public class EmailHelperService : IEmailHelperService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ClientSenderSettings _client;

        public EmailHelperService(IEmailService emailService,
            IOptions<ClientSenderSettings> client,
            UserManager<User> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
            _client = client.Value;
        }

     
        public async Task<ApiResponse> SendEmailConfirmationEmail(EmailSendRequest request)
        {
            var user = request.User;
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = GetCallbackUrl(user.Id, code, _client.EmailConfirmationPath,request.ClientUrl);
            var emailSendingSuccess = await _emailService.SendMessageWithPurpose(new MailModel()
            {
                ToEmail = user.Email,
                Subject = EmailPurpose.EmailConfirmation.ToString(),
            },
                EmailPurpose.EmailConfirmation,
                new ConfirmAccountEmailViewModel
                {
                    ConfirmEmailUrl = callbackUrl
                });

            if (!emailSendingSuccess)
            {
                return new ApiResponse(
                    new ApiError()
                    {
                        ErrorCode = ErrorCode.EmailSendFailed,
                        ErrorMessage = ErrorCode.EmailSendFailed.GetDescription(),
                    });
            }
            return ApiResponse.Success();
        }

        public async Task<ApiResponse> SendResetPasswordConfirmationEmail(EmailSendRequest request)
        {
            var user = request.User;
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = GetCallbackUrl(user.Id, code, _client.ResetPasswordPath,request.ClientUrl);
            var emailSendingSuccess = await _emailService.SendMessageWithPurpose(new MailModel()
            {
                ToEmail = user.Email,
                Subject = EmailPurpose.ResetPassword.ToString(),
            },
                EmailPurpose.ResetPassword,
                new ResetPasswordViewModel()
                {
                    ResetPasswordUrl = callbackUrl,
                    UserName = user.UserName,
                });

            if (!emailSendingSuccess)
            {
                return new ApiResponse(
                    new ApiError()
                    {
                        ErrorCode = ErrorCode.EmailSendFailed,
                        ErrorMessage = ErrorCode.EmailSendFailed.GetDescription(),
                    });
            }
            return ApiResponse.Success();
        }
        private  const string  RouteEnd = "/";
        private string GetCallbackUrl(Guid userId, string token, string path, string? clientUrl= null)
        {
            var url = $"{_client.Url}{path}";
            if (!String.IsNullOrEmpty(clientUrl))
            {
                url = clientUrl.EndsWith(RouteEnd )? clientUrl: $"{clientUrl}{RouteEnd}";
            }
            var callbackUrl =
                $"{url}?{nameof(ISendEmailCodeRequest.UserId)}={userId}&{nameof(ISendEmailCodeRequest.Code)}={WebUtility.UrlEncode(token)}";
            return callbackUrl;
        }
    }
}