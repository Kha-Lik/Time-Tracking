using System;
using TimeTracking.Common.Repository;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Repositories
{
    public class MilestoneRepository : Repository<Guid, Milestone, TimeTrackingDbContext>, IMilestoneRepository
    {
        public MilestoneRepository(TimeTrackingDbContext dbContext) : base(dbContext)
        {
        }
    }
}