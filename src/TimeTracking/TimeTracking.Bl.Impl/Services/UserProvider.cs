using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Bl.Impl.Exceptions;
using TimeTracking.Common.Jwt;

namespace TimeTracking.Bl.Impl.Services
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid GetUserId()
        {
            var userId = _context.HttpContext.User.Claims
                .First(i => i.Type == Constants.Strings.JwtClaimIdentifiers.Id).Value;
            var result = Guid.TryParse(userId, out Guid id);
            if (!result)
            {
                throw new AuthorizationError("Failed to fetch user id");
            }
            return id;
        }
    }
}