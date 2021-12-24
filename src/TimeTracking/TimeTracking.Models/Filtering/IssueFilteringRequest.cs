using System;
namespace TimeTracking.Models.Filtering
{
    public class IssueFilteringRequest
    {
        public  Guid MilestoneId { get; set; }
        public  DateTime? StartDate { get; set; }
        public  DateTime? EndDate { get; set; }
    }
}