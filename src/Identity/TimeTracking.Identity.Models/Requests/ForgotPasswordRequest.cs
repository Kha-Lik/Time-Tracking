namespace TimeTracking.Identity.Models.Requests
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public  string ClientUrl { get; set; }
    }
}