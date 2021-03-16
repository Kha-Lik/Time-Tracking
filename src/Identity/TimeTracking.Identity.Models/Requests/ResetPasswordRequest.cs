using System;

namespace TimeTracking.Identity.Models.Requests
{
    public class ResetPasswordRequest: ISendEmailCodeRequest
    {
        public  Guid UserId { get; set; }
        public  string Code { get; set; }
        public  string Password { get; set; }
    }

    public interface ISendEmailCodeRequest
    {
        public  Guid UserId { get; set; }
        public  string Code { get; set; }
    }
}