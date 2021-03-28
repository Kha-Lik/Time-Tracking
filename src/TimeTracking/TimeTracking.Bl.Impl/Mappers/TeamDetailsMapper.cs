using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class TeamDetailsMapper : IModelMapper<Team, TeamDetailsDto>
    {
        public TeamDetailsDto MapToModel(Team entity)
        {
            return new TeamDetailsDto()
            {
                MembersCount = entity.MembersCount,
                Name = entity.Name,
                ProjectAbbreviation = entity.Project?.Abbreviation,
                ProjectId = entity.ProjectId,
                ProjectName = entity.Project?.Name,
                TeamId = entity.Id,
            };
        }
    }
}