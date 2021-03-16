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
        
        public async Task<Tuple<string,List<WorkLog>>> GetActivitiesWithDetailsByUserId(Guid userId, Guid projectId)
        {
            var workLogSubQuery =
                from wl in _dbContext.WorkLogs.Where(e => e.UserId.Equals(userId))
                join i in _dbContext.Issues on wl.IssueId equals i.Id
                join pr in _dbContext.Projects on i.ProjectId equals projectId
                select new {wl,i,pr};
            
           var query = from w in  workLogSubQuery
                group w by new {ProjectName = w.pr.Name, Id =w.pr.Id} into grp
                select new Tuple<string,List<WorkLog>>(grp.Key.ProjectName,grp.Select(e=>e.wl).ToList());
            return  await query.FirstOrDefaultAsync();
        }
    }
}