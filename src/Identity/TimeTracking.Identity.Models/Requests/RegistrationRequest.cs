using System.ComponentModel.DataAnnotations;

namespace TimeTracking.Identity.Models.Requests
{
    public class RegistrationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  string ClientUrl { get; set; }
    }
}