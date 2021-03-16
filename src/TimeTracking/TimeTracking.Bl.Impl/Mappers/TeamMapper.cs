using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class TeamMapper:IBaseMapper<Team,TeamDto>
    {
        public TeamDto MapToModel(Team entity)
        {
            return new TeamDto()
            {
                MembersCount = entity.MembersCount,
                Name = entity.Name,
                ProjectId = entity.ProjectId,
            };
        }

        public Team MapToEntity(TeamDto model)
        {
            return new Team()
            {
                MembersCount = model.MembersCount,
                Name = model.Name,
                ProjectId = model.ProjectId,
            };
        }
    }
}