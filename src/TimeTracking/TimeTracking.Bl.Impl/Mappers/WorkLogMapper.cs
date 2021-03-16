using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class WorkLogMapper:IBaseMapper<WorkLog,WorkLogDto>
    {
        public WorkLogDto MapToModel(WorkLog entity)
        {
            return new WorkLogDto()
            {
                StartDate = entity.StartDate,
                TimeSpent = entity.TimeSpent,
                Description = entity.Description,
                ActivityType = entity.ActivityType,
                IssueId = entity.IssueId,
            };
        }

        public WorkLog MapToEntity(WorkLogDto model)
        {
            return new WorkLog()
            {
                StartDate = model.StartDate,
                TimeSpent = model.TimeSpent,
                Description = model.Description,
                ActivityType = model.ActivityType,
            };
        }
    }
}