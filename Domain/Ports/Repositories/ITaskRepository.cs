using Domain.Entities;

namespace Domain.Ports.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(string? priority = null);
        Task<TaskItem?> FindTaskAsync(long id);
    }
}
