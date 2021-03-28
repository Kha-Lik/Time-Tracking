using System;
using System.Threading.Tasks;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Abstract.Repository;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Abstract.Repositories
{
    public interface ITeamRepository : IRepository<Guid, Team>
    {
        Task<Team> GetByIdWithDetails(Guid teamId);
    }
}