using System;
using TimeTracking.Common.Enums;

namespace TimeTracking.Models.Requests
{
    public class  WorkLogUpdateRequest
    {
        public  string Description { get; set; }
        public  Guid WorkLogId { get; set; }
        public  TimeSpan TimeSpent { get; set; }
        public  ActivityType ActivityType { get; set; }
        public  DateTimeOffset StartDate { get; set; }
    }
}