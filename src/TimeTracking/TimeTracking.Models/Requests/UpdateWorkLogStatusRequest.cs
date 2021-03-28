#nullable enable
using System;

namespace TimeTracking.Models.Requests
{
    public class UpdateWorkLogStatusRequest
    {
        public Guid WorkLogId { get; set; }
        public bool IsApproved { get; set; }
        public string? Description { get; set; }
    }
}