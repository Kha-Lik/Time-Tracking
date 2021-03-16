using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using TimeTracking.Common.Abstract.Repository;

namespace TimeTracking.Common.Repository
{
    public class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWorkFactory(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWork Create(IsolationLevel isolationLevel)
        {
            return new UnitOfWork(_dbContext, isolationLevel);
        }

        public IUnitOfWork Create()
        {
            return Create(IsolationLevel.ReadCommitted);
        }
    }
}