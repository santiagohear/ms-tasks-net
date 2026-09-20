using Microsoft.EntityFrameworkCore;
using Domain.Ports.Repositories;

namespace Infrastructure.Adapters.Repositories
{
    public class PaginatedResult<TEntity> : IPaginatedResult<TEntity>
    {
        public int TotalCount { get; private set; }
        public int PageCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public IEnumerable<TEntity> Items { get; private set; } = [];

        public PaginatedResult(int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            if (PageSize < 1)
            {
                PageSize = IPaginatedResult<TEntity>.DefaultPageSize;
            }

            if (PageIndex < 0)
            {
                PageIndex = IPaginatedResult<TEntity>.DefaultPageIndex;
            }
        }

        public PaginatedResult(int pageIndex, int pageSize, int totalCount, IEnumerable<TEntity> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = items;
        }

        public async Task<IPaginatedResult<TEntity>> PaginateAsync(IQueryable<TEntity> query)
        {
            TotalCount = await query.CountAsync();
            PageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = await query.Skip(PageIndex * PageSize).Take(PageSize).ToArrayAsync();

            return this;
        }

        public IPaginatedResult<T> CreateFrom<T>(IEnumerable<T> items)
        {
            return new PaginatedResult<T>(PageIndex, PageSize)
            {
                TotalCount = TotalCount,
                PageCount = PageCount,
                Items = items
            };
        }
    }
}
