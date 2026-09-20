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

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(string? priority = null)
        {
            var query = DbContext.Set<TaskItem>()
                .Include(x => x.AssignedToUser)
                .Include(x => x.CreatedByUser)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(priority))
            {
                query = query.Where(x => JsonDbFunctions.JsonValue(x.AdditionalInfoJson, "$.prioridad") == priority);
            }

            return await query.ToListAsync();
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
