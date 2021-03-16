using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class MilestoneDetailsMapper:IModelMapper<Milestone,MilestoneDetailsDto>
    {
        public MilestoneDetailsDto MapToModel(Milestone entity)
        {
            return new MilestoneDetailsDto()
            {
                State = entity.State,
                Description = entity.Description,
                Title = entity.Title,
                DueDate = entity.DueDate,
                ProjectId = entity.ProjectId,
                Id = entity.Id,
            };
        }
    }
}