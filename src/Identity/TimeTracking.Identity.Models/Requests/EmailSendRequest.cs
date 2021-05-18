#nullable enable
using TimeTracking.Identity.Entities;

namespace TimeTracking.Identity.Models.Requests
{
    public class  EmailSendRequest
    {
        public  User User { get; set; }
        public  string? ClientUrl { get; set; }
    }
}