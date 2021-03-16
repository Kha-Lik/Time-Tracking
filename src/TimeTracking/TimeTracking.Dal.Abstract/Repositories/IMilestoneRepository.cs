using System;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Abstract.Repository;
using TimeTracking.Entities;

namespace TimeTracking.Dal.Abstract.Repositories
{
    public interface IMilestoneRepository:IRepository<Guid,Milestone>
    {
        
    }
}