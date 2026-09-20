using Domain.Entities;

namespace Domain.Ports.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<TaskItem?> FindTaskAsync(long id);
    }
}
