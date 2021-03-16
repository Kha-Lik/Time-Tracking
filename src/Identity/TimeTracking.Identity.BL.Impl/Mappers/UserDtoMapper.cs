using TimeTracking.Common.Mappers;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Dtos;

namespace TimeTracking.Identity.BL.Impl.Mappers
{
    public class UserDtoMapper:IBaseMapper<User,UserDto>
    {
        public UserDto MapToModel(User entity)
        {
            return new UserDto()
            {
                Id = entity.Id,
                FirstName = entity.LastName,
                LastName = entity.LastName,
                Email = entity.Email,
                Phone = entity.PhoneNumber,
            };
        }

        public User MapToEntity(UserDto model)
        {
            return new User()
            {
                FirstName = model.LastName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
            };
        }
    }
}