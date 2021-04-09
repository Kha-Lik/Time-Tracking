using System.Runtime.Serialization;
#nullable  enable
namespace TimeTracking.Identity.Models.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        [DataMember(EmitDefaultValue = false)]
        public long ExpiredAt { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string? Message { get; set; }
    }
}
#nullable restore