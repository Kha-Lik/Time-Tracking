using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TimeTracking.Identity.BL.Abstract.Services
{
    public interface IJwtTokenHandler
    {
        public string WriteToken(JwtSecurityToken jwt);
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}