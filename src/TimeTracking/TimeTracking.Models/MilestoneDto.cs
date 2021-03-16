using System;
using TimeTracking.Entities;

namespace TimeTracking.Models
{
    public class  MilestoneDto 
    {
        public State State { get; set; }
        public string Title{ get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate{ get; set; }
        public Guid ProjectId { get; set; }
    }

    public class MilestoneDetailsDto:MilestoneDto
    {
        public  Guid Id { get; set; }
    }
    
}