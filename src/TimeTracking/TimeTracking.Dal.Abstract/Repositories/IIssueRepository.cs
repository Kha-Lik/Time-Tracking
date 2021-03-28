using System;
using System.Threading.Tasks;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Abstract.Repository;
using TimeTracking.Common.Pagination;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Abstract.Repositories
{
    public interface IIssueRepository : IRepository<Guid, Issue>
    {
        Task<Issue> GetIssueWithDetails(Guid id);
        Task<PagedResult<Issue>> GetAllIssueWithDetails(int page, int size);
    }
}