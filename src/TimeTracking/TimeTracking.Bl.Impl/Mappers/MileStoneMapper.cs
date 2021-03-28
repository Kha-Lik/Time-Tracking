using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class MileStoneMapper : IBaseMapper<Milestone, MilestoneDto>
    {
        public MilestoneDto MapToModel(Milestone entity)
        {
            return new MilestoneDto()
            {
                State = entity.State,
                Description = entity.Description,
                Title = entity.Title,
                DueDate = entity.DueDate,
                ProjectId = entity.ProjectId,
            };
        }

        public Milestone MapToEntity(MilestoneDto model)
        {
            return new Milestone()
            {
                State = model.State,
                Description = model.Description,
                Title = model.Title,
                DueDate = model.DueDate,
                ProjectId = model.ProjectId,
            };
        }
    }
}