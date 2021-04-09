using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TimeTracking.Identity.BL.Abstract.Validators
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
    }
}