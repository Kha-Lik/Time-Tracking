using System;
using TimeTracking.Entities;

namespace TimeTracking.Models
{
    public class TimeTrackingUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid? TeamId { get; set; }
    }

    public class TimeTrackingUserDetailsDto : TimeTrackingUserDto
    {
        public Guid UserId { get; set; }
    }
}