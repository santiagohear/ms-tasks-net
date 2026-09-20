using Domain.Entities;

namespace Domain.Ports.Services
{
    public interface ITaskService
    {
        Task CreateTaskAsync(TaskItem? task);
        Task<IEnumerable<TaskItem>> GetTasksAsync(string? priority = null);
        Task UpdateTaskStatusAsync(long taskId, string status);
        Task UpdateTaskPriorityAsync(long taskId, string priority);
    }
}
