using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TimeTracking.Common.Abstract.Repository
{
    public interface IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        IUnitOfWork Create();
        IUnitOfWork Create(IsolationLevel isolationLevel);
    }
}