using System;
using System.Linq;
using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class IssueDetailsMapper : IModelMapper<Issue, IssueDetailsDto>
    {
        public IssueDetailsDto MapToModel(Issue entity)
        {
            return new IssueDetailsDto()
            {
                Status = entity.Status,
                ProjectName = entity.Project?.Abbreviation,
                ClosedAt = entity.ClosedAt,
                Description = entity.Description,
                Title = entity.Title,
                OpenedAt = entity.OpenedAt,
                UpdatedAt = entity.UpdatedAt,
                IssueId = entity.Id,
                AssignedToUserId = entity.AssignedToUserId,
                AssignedUserFirstName = entity.TimeTrackingUserAssigned?.FirstName,
                AssignedUserLastName = entity.TimeTrackingUserAssigned?.LastName,
                ReportedByLastName = entity.TimeTrackingUserReporter?.LastName,
                ReportedByUserFirstName = entity.TimeTrackingUserReporter?.FirstName,
                MileStoneName = entity.Milestone?.Title,
                TotalRemainingTimeInSeconds = Math.Abs((long)(entity.OpenedAt - entity.CreatedAt).TotalSeconds),
                TotalSpentTimeInSeconds = entity?.WorkLogs?.Sum(e => e.TimeSpent)??0,
            };
        }
    }
}