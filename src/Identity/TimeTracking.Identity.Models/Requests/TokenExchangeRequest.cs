using System.ComponentModel.DataAnnotations;

namespace TimeTracking.Identity.Models.Requests
{
    public class TokenExchangeRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}