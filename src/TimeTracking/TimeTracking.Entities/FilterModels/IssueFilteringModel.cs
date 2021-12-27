using System;
namespace TimeTracking.Entities.FilterModels
{
    public class IssueFilteringModel
    {
        public  Guid? MilestoneId { get; set; }
        public  DateTime? StartDate { get; set; }
        public  DateTime? EndDate { get; set; }
    }
}