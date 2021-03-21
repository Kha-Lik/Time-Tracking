using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Common.Repository;
using TimeTracking.Dal.Abstract.Repositories;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Impl.Repositories
{
    public class WorklogRepository:Repository<Guid,WorkLog,TimeTrackingDbContext>,IWorklogRepository
    {
        public WorklogRepository(TimeTrackingDbContext dbContext) : base(dbContext)
        {
        }
        
        public async Task<List<WorkLog>> GetActivitiesWithDetailsByUserId(Guid userId, Guid projectId)
        {
     
            var query =
                _dbContext.WorkLogs.Where(e => e.UserId.Equals(userId))
                    .Include(e => e.Issue)
                    .ThenInclude(e => e.Project)
                    .Where(e=>e.Issue.ProjectId.Equals(projectId));
            return  await query.ToListAsync();
        }

        public async Task<WorkLog> GetByIdWithUserAsync(Guid workLogId)
        {
             return await _dbContext.WorkLogs.Where(e => e.Id.Equals(workLogId))
                .Include(e => e.TimeTrackingUser)
                .FirstOrDefaultAsync();
        }
    }
}