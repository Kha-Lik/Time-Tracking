using System;

namespace TimeTracking.Models
{
    public class TeamDetailsDto:TeamDto
    {
        public Guid TeamId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAbbreviation { get; set; }
    }
}