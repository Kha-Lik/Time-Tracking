using System;

namespace TimeTracking.Models
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTimeOffset InitialRisk { get; set; }
    }

    public class ProjectDetailsDto : ProjectDto
    {
        public Guid ProjectId { get; set; }
    }
}