using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TimeTracking.Identity.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset RegistrationDate { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}