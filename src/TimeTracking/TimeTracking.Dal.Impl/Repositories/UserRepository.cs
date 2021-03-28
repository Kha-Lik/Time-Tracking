using System;
using TimeTracking.Common.Repository;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Repositories
{
    public class UserRepository : Repository<Guid, TimeTrackingUser, TimeTrackingDbContext>, IUserRepository
    {
        public UserRepository(TimeTrackingDbContext dbContext) : base(dbContext)
        {
        }


    }
}