using System;
using System.Collections.Generic;

namespace TimeTracking.Models
{
    public class UserActivityDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail{ get; set; }
        public long TotalWorkLogInSeconds { get; set; }
        public string ProjectName { get; set; }
        public List<WorkLogDto> WorkLogItems { get; set; }
    }

    public class WorkLogItemDto
    {
        public string Description { get; set; }
        public string IssueName { get; set; }
        public Guid IssueId { get; set; }
        public TimeSpan TimeSpent { get; set; }
    }
}