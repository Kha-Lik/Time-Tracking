using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Common.Repository;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Repositories
{
    public class TeamRepository:Repository<Guid,Team,TimeTrackingDbContext>,ITeamRepository
    {
        public TeamRepository(TimeTrackingDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Team> GetByIdWithDetails(Guid teamId)
        {
            return _dbContext.Teams
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e=>e.Id==teamId);
        }
    }
}