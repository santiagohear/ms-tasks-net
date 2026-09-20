using System.Linq.Expressions;
using Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Base;
using Domain.Ports.Repositories;
using Infrastructure.DataSource.StoreProcedures;
using Infrastructure.DataSource.Helpers;

namespace Infrastructure.Adapters.Repositories
{
    public class Repository<TEntity>(PersistenceContext context) : IRepository<TEntity>, IDisposable where TEntity : DomainEntity
    {
        private readonly PersistenceContext _context = context ?? throw new ArgumentNullException(nameof(context));

        internal DbContext DbContext => _context;

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity), "La entidad no puede estar vacia");

            _context.Set<TEntity>().Add(entity);

            await SaveAsync();

            return entity;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _context.Set<TEntity>().Remove(entity);

            await SaveAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeStringProperties = "", bool isTracking = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeStringProperties))
            {
                foreach (var includeProperty in includeStringProperties.Split
                    ([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool isTracking = false, params Expression<Func<TEntity, object>>[] includeObjectProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties != null)
            {
                foreach (var include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<TEntity?> FindAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _context.Set<TEntity>().Update(entity);

            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            _context.ChangeTracker.DetectChanges();

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public async Task<IPaginatedResult<TEntity>> GetPaginatedAsync(int pageIndex = IPaginatedResult<TEntity>.DefaultPageIndex,
            int pageSize = IPaginatedResult<TEntity>.DefaultPageSize,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<TEntity, object>>[] includeObjectProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties is not null)
            {
                foreach (var include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var paginatedResult = new PaginatedResult<TEntity>(pageIndex, pageSize);

            return await paginatedResult.PaginateAsync(query);
        }

        public async Task<StoreProcedureResult<TEntity>> ExecuteStoreProcedureAsync(string storeProcedureName)
        {
            var items = await _context.Set<TEntity>().FromSqlRaw(storeProcedureName).ToListAsync();

            return new StoreProcedureResult<TEntity>(items);
        }

        public async Task<StoreProcedureResult<TEntity>> ExecuteStoreProcedureAsync<TDefinition>(string storeProcedureName, TDefinition definition)
        {
            var (query, parameters) = definition.AsStoreProcedure(storeProcedureName);

            var items = await _context.Set<TEntity>().FromSqlRaw(query, [.. parameters]).ToListAsync();

            return new StoreProcedureResult<TEntity>(items, parameters.GetOutputValues<TDefinition>());
        }

        public async Task<StoreProcedureResult<TEntity>> ExecuteStoreProcedureAsync<TDefinition>(string storeProcedureName, object instance)
        {
            var (query, parameters) = instance.AsStoreProcedure<TDefinition>(storeProcedureName);

            var items = await _context.Set<TEntity>().FromSqlRaw(query, [.. parameters]).ToListAsync();

            return new StoreProcedureResult<TEntity>(items, parameters.GetOutputValues<TDefinition>());
        }

        public async Task<StoreProcedureResult> ExecuteSqlRawAsync<TDefinition>(string storeProcedureName, object instance)
        {
            var (query, parameters) = instance.AsStoreProcedure<TDefinition>(storeProcedureName);

            await _context.Database.ExecuteSqlRawAsync(query, parameters);

            return new StoreProcedureResult(parameters.GetOutputValues<TDefinition>());
        }

        public async Task<StoreProcedureResult> ExecuteSqlRawAsync<TDefinition>(string storeProcedureName, TDefinition definition)
        {
            var (query, parameters) = definition.AsStoreProcedure(storeProcedureName);

            await _context.Database.ExecuteSqlRawAsync(query, [.. parameters]);

            return new StoreProcedureResult(parameters.GetOutputValues<TDefinition>());
        }
    }
}
