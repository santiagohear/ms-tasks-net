namespace Domain.Ports.Repositories
{
    public interface IPaginatedResult<TEntity>
    {
        const int DefaultPageSize = 10;
        const int DefaultPageIndex = 0;

        int TotalCount { get; }
        int PageCount { get; }
        int PageIndex { get; }
        int PageSize { get; }
        IEnumerable<TEntity> Items { get; }

        IPaginatedResult<T> CreateFrom<T>(IEnumerable<T> items);
    }
}
