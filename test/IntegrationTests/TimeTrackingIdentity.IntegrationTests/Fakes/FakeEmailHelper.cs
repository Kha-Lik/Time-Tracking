using System.Threading.Tasks;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Email;
using TimeTracking.Common.Enums;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Impl.Services;
using TimeTracking.Identity.Entities;

namespace TimeTrackingIdentity.IntegrationTests.Fakes
{
    public class FakeEmailHelper : IEmailHelperService
    {
        public bool SendSuccessfully { get; set; } = true;

        public async Task<ApiResponse> SendEmailConfirmationEmail(User user)
        {
            return new ApiResponse() {IsSuccess = SendSuccessfully};
        }

        public async Task<ApiResponse> SendResetPasswordConfirmationEmail(User user)
        {
            return new ApiResponse() {IsSuccess = SendSuccessfully};
        }
    }
}