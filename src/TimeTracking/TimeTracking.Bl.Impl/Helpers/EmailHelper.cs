using System.Threading.Tasks;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Email;
using TimeTracking.Common.Enums;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Wrapper;
using TimeTracking.Templates.Models;

namespace TimeTracking.Bl.Impl.Helpers
{
    public interface IEmailHelper
    {
        Task<ApiResponse> SendEmailWithValidationOfWorkLogFailed(string email,string description);
    }

    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailService _emailService;

        public EmailHelper(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        public async Task<ApiResponse> SendEmailWithValidationOfWorkLogFailed(string email,string description)
        {

            var emailSendingSuccess = await _emailService.SendMessage(new MailModel()
            {
                ToEmail = email,
                Subject = EmailPurpose.ValidationOfWorkLogFailed.ToString(),
                Body = description

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
            return  ApiResponse.Success();
        }

    }
}