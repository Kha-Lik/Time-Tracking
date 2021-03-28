using System;
using TimeTracking.Common.Repository;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Repositories
{
    public class ProjectRepository : Repository<Guid, Project, TimeTrackingDbContext>, IProjectRepository
    {
        public ProjectRepository(TimeTrackingDbContext dbContext) : base(dbContext)
        {
        }
    }
}