using System;

namespace TimeTracking.Identity.Models.Requests
{
    public class AddToRoleRequest
    {
        public  Guid UserId { get; set; }
        public string RoleName { get; set; }
    }
}