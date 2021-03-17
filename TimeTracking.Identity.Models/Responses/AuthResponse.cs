using System.Runtime.Serialization;

#nullable  enable
namespace TimeTracking.Identity.Models.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public  long ExpiredAt { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public  string? Message { get; set; }
    }
}
#nullable restore