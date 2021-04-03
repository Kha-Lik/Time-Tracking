using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.BL.Abstract.Factories;
using TimeTracking.Identity.BL.Abstract.Services;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Identity.Models.Responses;

namespace TimeTracking.Identity.BL.Impl.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly ILogger<TokenService> _logger;

        public TokenService(UserManager<User> userManager,
            IJwtFactory jwtFactory,
            ILogger<TokenService> logger)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(TokenExchangeRequest request)
        {
            var authResponse = new AuthResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                authResponse.Message = $"No Accounts Registered with {request.Email}.";
                return authResponse;
            }
            // Only allow login if email is confirmed
            if (!user.EmailConfirmed)
            {
                authResponse.Message = $"Current email {request.Email} is not confirmed.";
                return authResponse;
            }
            // Used as user lock
            if (user.LockoutEnd >= DateTimeOffset.UtcNow)
            {
                authResponse.Message = $"This account has been locked.";
                return authResponse;
            }

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var jwtSecurityToken = await _jwtFactory.GenerateEncodedAccessToken(user);
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
                    authResponse.RefreshToken = activeRefreshToken!.Token;
                    authResponse.ExpiredAt = (activeRefreshToken.Expires - DateTimeOffset.UtcNow).Seconds;
                }
                else
                {
                    var refreshToken = _jwtFactory.GenerateRefreshToken();
                    authResponse.RefreshToken = refreshToken.Token;
                    authResponse.ExpiredAt = (refreshToken.Expires - DateTimeOffset.UtcNow).Seconds;
                    user.RefreshTokens.Add(refreshToken);
                    var updateResponse = await _userManager.UpdateAsync(user);
                    if (!updateResponse.Succeeded)
                    {
                        _logger.LogWarning("Updating user with id {0} failed with reason {1}", user.Id, updateResponse.ToString());
                        authResponse.Message = $"Failed to generate refresh token.";
                    }
                }

                authResponse.Token = jwtSecurityToken.Token;
                return authResponse;
            }

            authResponse.Message = $"Incorrect Credentials for user {user.Email}.";
            return authResponse;
        }

        public async Task<AuthResponse> RefreshTokenAsync(string token)
        {
            var authenticationModel = new AuthResponse();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authenticationModel.Message = $"Token did not match any users.";
                return authenticationModel;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                authenticationModel.Message = $"Token Not Active.";
                return authenticationModel;
            }
            refreshToken.Revoked = DateTime.UtcNow;

            var newRefreshToken = _jwtFactory.GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            var updateResponse = await _userManager.UpdateAsync(user);
            if (!updateResponse.Succeeded)
            {
                _logger.LogWarning("Updating user with id {0} failed with reason {1}", user.Id, updateResponse.ToString());
                authenticationModel.Message = $"Failed to generate refresh token.";
            }

            authenticationModel.Token = (await _jwtFactory.GenerateEncodedAccessToken(user)).Token;
            authenticationModel.RefreshToken = newRefreshToken.Token;
            authenticationModel.ExpiredAt = (refreshToken.Expires - DateTimeOffset.UtcNow).Seconds;
            return authenticationModel;
        }

        public async Task<AuthResponse> RevokeTokenAsync(string token)
        {
            var authResponse = new AuthResponse();
            authResponse.Message = ErrorCode.RefreshTokenRevocationFailed.GetDescription();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return authResponse;
            }
            var refreshToken = user.RefreshTokens.First(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                return authResponse;
            }
            refreshToken.Revoked = DateTimeOffset.UtcNow;
            var updateResponse = await _userManager.UpdateAsync(user);
            if (!updateResponse.Succeeded)
            {
                _logger.LogWarning("Updating user with id {0} failed with reason {1}", user.Id, updateResponse.ToString());
                return authResponse;
            }

            authResponse.Message = "Token revoked successfully";
            return authResponse;
        }

    }
}