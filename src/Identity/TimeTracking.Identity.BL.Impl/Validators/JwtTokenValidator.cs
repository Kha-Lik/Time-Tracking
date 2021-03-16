using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TimeTracking.Identity.BL.Abstract.Validators;

namespace TimeTracking.Identity.BL.Impl.Validators
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ILogger<JwtTokenValidator> _logger;

        public JwtTokenValidator(JwtSecurityTokenHandler jwtSecurityTokenHandler,
            ILogger<JwtTokenValidator> logger)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _logger = logger;
        }
      
        
        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError($"Token validation failed: {e.Message}");
                return null;
            }
        }
    }
}