using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace TimeTracking.Tests.Common.Helpers
{
    internal static class MockJwtTokens
    {
        public static string Issuer { get; } = Guid.NewGuid().ToString();
        public static string Audince { get; } = Guid.NewGuid().ToString();
        public static SecurityKey SecurityKey { get; }
        private static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static readonly RandomNumberGenerator random = RandomNumberGenerator.Create();
        private static readonly byte[] key = new byte[32];

        static MockJwtTokens()
        {
            random.GetBytes(key);
            SecurityKey = new SymmetricSecurityKey(key) { KeyId = Guid.NewGuid().ToString() };
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public static string GenerateJwtToken(IEnumerable<Claim> claims = null)
        {
            return tokenHandler.WriteToken(
                new JwtSecurityToken(Issuer, null, claims, null,
                DateTime.UtcNow.AddMinutes(20), SigningCredentials));
        }
    }
}