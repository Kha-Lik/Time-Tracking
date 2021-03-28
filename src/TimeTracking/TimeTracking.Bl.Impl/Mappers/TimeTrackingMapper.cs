using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class TimeTrackingMapper : IBaseMapper<TimeTrackingUser, TimeTrackingUserDto>
    {
        public TimeTrackingUserDto MapToModel(TimeTrackingUser entity)
        {
            return new TimeTrackingUserDto()
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                TeamId = entity.TeamId,
            };
        }

        public TimeTrackingUser MapToEntity(TimeTrackingUserDto model)
        {
            return new TimeTrackingUser()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TeamId = model.TeamId,
            };
        }
    }
}