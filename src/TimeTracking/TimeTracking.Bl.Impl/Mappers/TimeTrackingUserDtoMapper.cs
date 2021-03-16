using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class TimeTrackingUserDtoMapper:IModelMapper<TimeTrackingUser,TimeTrackingUserDetailsDto>
    {
        public TimeTrackingUserDetailsDto MapToModel(TimeTrackingUser entity)
        {
            return new TimeTrackingUserDetailsDto()
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                TeamId = entity.TeamId,
                UserId = entity.Id,
            };
        }
    }
}