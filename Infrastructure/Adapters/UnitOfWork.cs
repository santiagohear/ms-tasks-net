using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Domain.Entities.Base;
using Domain.Ports.Repositories;
using Infrastructure.Adapters.Repositories;

namespace Infrastructure.Adapters
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext? _initialDbContext;
        private IDbContextTransaction? _transaction;

        public async Task BeginAsync<T>(IRepository<T> repository) where T : DomainEntity
        {
            _initialDbContext = ((Repository<T>)repository).DbContext;
            _transaction = await _initialDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No se ha uniciado una transacción");
            }

            await _transaction.CommitAsync();
        }

        public async Task CreateSavepointAsync(string name)
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No se ha uniciado una transacción");
            }

            await _transaction.CreateSavepointAsync(name);
        }

        public async Task RollbackAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No se ha uniciado una transacción");
            }

            await _transaction.RollbackAsync();
        }

        public async Task RollbackToSavepointAsync(string name)
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No se ha uniciado una transacción");
            }

            await _transaction.RollbackToSavepointAsync(name);
        }

        public async Task UseAsync<T>(IRepository<T> repository) where T : DomainEntity
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No se ha uniciado una transacción");
            }

            var dbContext = ((Repository<T>)repository).DbContext;

            if (dbContext.Database.CurrentTransaction is not null)
            {
                return;
            }

            await dbContext.Database.UseTransactionAsync(_transaction.GetDbTransaction());
        }
    }
}
