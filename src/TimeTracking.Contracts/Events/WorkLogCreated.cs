using System;
using TimeTracking.Common.Enums;

namespace TimeTracking.Contracts.Events
{
    public interface WorkLogCreated
    {
        public DateTimeOffset StartDate { get; set; }
        public string ProjectName { get; set; }
        public Guid WorkLogId { get; set; }
        public Guid UserId { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}