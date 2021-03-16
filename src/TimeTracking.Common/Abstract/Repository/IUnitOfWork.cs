using System;
using System.Threading.Tasks;

namespace TimeTracking.Common.Abstract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}