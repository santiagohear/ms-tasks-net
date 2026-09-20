using Domain.Entities;

namespace Domain.Ports.Services
{
    public interface ITaskService
    {
        Task CreateTaskAsync(TaskItem? task);
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task UpdateTaskStatusAsync(long taskId, string status);
    }
}
