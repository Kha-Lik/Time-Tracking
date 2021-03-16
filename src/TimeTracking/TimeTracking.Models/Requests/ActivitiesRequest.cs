using System;

namespace TimeTracking.Models.Requests
{
    public class  ActivitiesRequest
    {
        public  Guid UserId { get; set; }
        public  Guid ProjectId { get; set; }
    }
}