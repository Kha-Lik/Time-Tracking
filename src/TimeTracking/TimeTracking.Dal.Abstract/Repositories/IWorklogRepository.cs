using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Abstract.Repository;
using TimeTracking.Common.Pagination;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Dal.Abstract.Repositories
{
    public interface IWorklogRepository:IRepository<Guid,WorkLog>
    {
        Task<List<WorkLog>> GetActivitiesWithDetailsByUserId(Guid userId, Guid projectId);
        Task<WorkLog> GetByIdWithUserAsync(Guid workLogId);
    }
}