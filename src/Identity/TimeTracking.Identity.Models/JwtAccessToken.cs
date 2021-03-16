namespace TimeTracking.Identity.Models
{
    public class JwtAccessToken
    {
        public  string Token { get; set; }
        public  long ExpiredAt { get; set; }
    }
}