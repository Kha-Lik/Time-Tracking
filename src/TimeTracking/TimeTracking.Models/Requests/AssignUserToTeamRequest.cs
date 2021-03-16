using System;

namespace TimeTracking.Models.Requests
{
    public class AssignUserToTeamRequest
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
    }
}