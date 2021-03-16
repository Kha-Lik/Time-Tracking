using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class IssueDetailsMapper:IModelMapper<Issue,IssueDetailsDto>
    {
        public IssueDetailsDto MapToModel(Issue entity)
        {
            return new IssueDetailsDto()
            {
                Status = entity.Status,
                ClosedAt = entity.ClosedAt,
                Description = entity.Description,
                Title = entity.Title,
                OpenedAt = entity.OpenedAt,
                UpdatedAt = entity.UpdatedAt,
                IssueId = entity.Id,
                AssignedUserFirstName = entity.TimeTrackingUserAssigned?.FirstName,
                AssignedUserLastName = entity.TimeTrackingUserAssigned?.LastName,
                ReportedByLastName = entity.TimeTrackingUserReporter?.LastName,
                ReportedByUserFirstName = entity.TimeTrackingUserReporter?.FirstName,
                MileStoneName = entity.Milestone?.Title,
            };
        }
    }

 
}