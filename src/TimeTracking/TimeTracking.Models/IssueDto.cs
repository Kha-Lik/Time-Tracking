using System;
using TimeTracking.Entities;

namespace TimeTracking.Models
{
    public class IssueDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public Guid ReportedByUserId { get; set; }
        public Guid? MilestoneId { get; set; }
        public Guid ProjectId { get; set; }

    }


}