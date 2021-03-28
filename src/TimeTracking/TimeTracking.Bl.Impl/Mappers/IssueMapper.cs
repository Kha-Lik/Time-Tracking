using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class IssueMapper : IBaseMapper<Issue, IssueDto>
    {
        public IssueDto MapToModel(Issue entity)
        {
            return new IssueDto()
            {
                Status = entity.Status,
                Description = entity.Description,
                AssignedToUserId = entity.AssignedToUserId,
                ReportedByUserId = entity.ReportedByUserId,
                Title = entity.Title,
            };
        }

        public Issue MapToEntity(IssueDto model)
        {
            return new Issue()
            {
                Status = model.Status,
                Description = model.Description,
                AssignedToUserId = model.AssignedToUserId,
                ReportedByUserId = model.ReportedByUserId,
                Title = model.Title,
            };
        }
    }
}