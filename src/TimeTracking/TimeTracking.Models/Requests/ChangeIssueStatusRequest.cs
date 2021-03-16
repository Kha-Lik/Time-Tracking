using System;
using TimeTracking.Entities;

namespace TimeTracking.Models.Requests
{
    public class  ChangeIssueStatusRequest
    {
        public  Status Status { get; set; }
        public  Guid IssueId { get; set; }
    }
}