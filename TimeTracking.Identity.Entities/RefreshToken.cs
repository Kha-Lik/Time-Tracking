using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TimeTracking.Identity.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTimeOffset  Expires { get; set; }
        [NotMapped]
        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Revoked { get; set; }
        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;
    }
}