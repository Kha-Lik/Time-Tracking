using System;

namespace TimeTracking.Models.Requests
{
    public class  AssignIssueToUserRequest
    {
        public Guid IssueId { get; set; }
        public Guid UserId { get;set; }
    }
}