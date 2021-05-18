using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class WorkLogDetailsMapper : IModelMapper<WorkLog, WorkLogDetailsDto>
    {
        public WorkLogDetailsDto MapToModel(WorkLog entity)
        {
            return new WorkLogDetailsDto()
            {
                StartDate = entity.StartDate,
                TimeSpent = entity.TimeSpent,
                Description = entity.Description,
                ActivityType = entity.ActivityType,
                IssueId = entity.IssueId,
                WorkLogId = entity.Id,
                UserId = entity.UserId,
                IsApproved = entity.IsApproved,
            };
        }
    }
}