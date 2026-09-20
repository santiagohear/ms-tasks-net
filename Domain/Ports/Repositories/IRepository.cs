using System.Linq.Expressions;
using Domain.Entities.Base;

namespace Domain.Ports.Repositories
{
    public interface IRepository<E> where E : DomainEntity
    {
        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null, string includeStringProperties = "",
            bool isTracking = false);

        Task<IEnumerable<E>> GetAsync(Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null,
            bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties);

        Task<E?> FindAsync(object id);
        Task<E> AddAsync(E entity);
        Task UpdateAsync(E entity);
        Task RemoveAsync(E entity);
        Task<IPaginatedResult<E>> GetPaginatedAsync(int pageIndex = IPaginatedResult<E>.DefaultPageIndex,
            int pageSize = IPaginatedResult<E>.DefaultPageSize,
            Expression<Func<E, bool>>? filter = null,
            Func<IQueryable<E>, IOrderedQueryable<E>>? orderBy = null,
            bool isTracking = false, params Expression<Func<E, object>>[] includeObjectProperties);
    }
}
