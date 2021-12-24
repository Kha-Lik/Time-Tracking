using System;
using TimeTracking.Entities;

namespace TimeTracking.Models
{
    public class IssueDetailsDto : IssueDto
    {
        public Guid IssueId { get; set; }
        public  string ProjectName { get; set; }
        public string AssignedUserFirstName { get; set; }
        public string AssignedUserLastName { get; set; }
        public string ReportedByUserFirstName { get; set; }
        public string ReportedByLastName { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset OpenedAt { get; set; }
        public DateTimeOffset ClosedAt { get; set; }
        public string MileStoneName { get; set; }
        public long TotalRemainingTimeInSeconds { get; set; }
        public long TotalSpentTimeInSeconds { get; set; }
    }
}