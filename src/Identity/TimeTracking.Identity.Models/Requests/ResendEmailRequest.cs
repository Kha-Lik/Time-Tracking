#nullable enable
namespace TimeTracking.Identity.Models.Requests
{
    public class ResendEmailRequest
    {
        public string Email { get; set; }
        public string? ClientUrl { get; set; }
    }
}