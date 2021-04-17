using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.BL.Abstract.Validators;
using TimeTracking.Identity.BL.Impl.Validators;

namespace TimeTracking.Identity.BL.Impl.Services
{
    public sealed class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly ILogger<JwtTokenHandler> _logger;
        private readonly IJwtTokenValidator _jwtTokenValidator;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtTokenHandler(ILogger<JwtTokenHandler> logger,
            IJwtTokenValidator jwtTokenValidator)
        {
            _jwtSecurityTokenHandler ??= new JwtSecurityTokenHandler();
            _logger = logger;
            _jwtTokenValidator = jwtTokenValidator;
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
        {
            return _jwtTokenValidator.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = false, //check expired tokens
            });
        }
    }
}