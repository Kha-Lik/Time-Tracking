using System;

namespace TimeTracking.Identity.Models.Requests
{
    public class EmailConfirmationRequest : ISendEmailCodeRequest
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}