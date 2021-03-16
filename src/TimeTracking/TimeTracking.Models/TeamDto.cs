using System;

namespace TimeTracking.Models
{
    public class TeamDto
    {
        public  string Name { get; set; }
        public  int MembersCount { get; set; }
        public  Guid ProjectId { get; set; }
    }
}