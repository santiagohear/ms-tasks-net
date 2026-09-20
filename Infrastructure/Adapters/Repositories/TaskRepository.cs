using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Attributes;
using Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Repositories
{
    [Adapter]
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(PersistenceContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            return await DbContext.Set<TaskItem>()
                .Include(x => x.AssignedToUser)
                .Include(x => x.CreatedByUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaskItem?> FindTaskAsync(long id)
        {
            return await DbContext.Set<TaskItem>()
                .Include(x => x.AssignedToUser)
                .Include(x => x.CreatedByUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
