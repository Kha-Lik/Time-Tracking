using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TimeTracking.Common.Abstract.Repository;

namespace TimeTracking.Common.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly DbContext _dbContext;
        IDbContextTransaction _dbContextTransaction;
        private readonly IsolationLevel _isolationLevel;
        private readonly bool _isTransactionStarted;

        public UnitOfWork(DbContext dbContext, IsolationLevel isolationLevel)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _isolationLevel = isolationLevel;
            if (_dbContext.Database.CurrentTransaction == null)
            {
                _dbContextTransaction = _dbContext.Database.BeginTransaction(_isolationLevel);
                _isTransactionStarted = true;
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
            if (_isTransactionStarted && _dbContextTransaction != null)
            {
                _dbContextTransaction.Commit();
                _dbContextTransaction = _dbContext.Database.BeginTransaction(_isolationLevel);
            }
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            if (_isTransactionStarted && _dbContextTransaction != null)
            {
                await _dbContextTransaction.CommitAsync();
                _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel);
            }
        }

        public void Rollback()
        {
            if (_isTransactionStarted)
                _dbContextTransaction?.Rollback();
        }
        public async Task RollbackAsync()
        {
            if (_isTransactionStarted && _dbContextTransaction != null)
                await _dbContextTransaction?.RollbackAsync();
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
        }
    }

    /*        public virtual async Task SaveChangesAsync()
              {
                try
                {
                    Validate();
                    var result =await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                    throw;
                }
            }

            private void Validate()
            {
                var entities = _dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                    .Select(e => e.Entity)
                    .ToList();

                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
                }
            }

    */

}