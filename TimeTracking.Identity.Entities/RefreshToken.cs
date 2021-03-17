using System;
using Microsoft.EntityFrameworkCore;

namespace TimeTracking.Identity.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTimeOffset  Expires { get; set; } = DateTimeOffset.UtcNow.AddMinutes(30);
        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}