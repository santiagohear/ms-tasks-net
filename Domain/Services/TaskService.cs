using Domain.Attributes;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports.Repositories;
using Domain.Ports.Services;

namespace Domain.Services
{
    [DomainService]
    public class TaskService(ITaskRepository taskRepository, IRepository<TaskItem> repository, IRepository<User> userRepository) : ITaskService
    {
        private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        private readonly IRepository<TaskItem> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly IRepository<User> _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        private static readonly HashSet<string> ValidStatuses =
        [
            TaskItem.PendingStatus,
            TaskItem.InProgressStatus,
            TaskItem.DoneStatus
        ];

        public async Task CreateTaskAsync(TaskItem? task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ValidationException("El título de la tarea es obligatorio");
            }

            TaskAdditionalInfo.EnsureValidJson(task.AdditionalInfoJson);

            var assignedUser = await _userRepository.FindAsync(task.AssignedToUserId);
            if (assignedUser is null)
            {
                throw new ValidationException($"No existe un usuario asignado con Id: {task.AssignedToUserId}");
            }

            var createdByUser = await _userRepository.FindAsync(task.CreatedByUserId);
            if (createdByUser is null)
            {
                throw new ValidationException($"No existe un usuario creador con Id: {task.CreatedByUserId}");
            }

            if (string.IsNullOrWhiteSpace(task.Status))
            {
                task.Status = TaskItem.PendingStatus;
            }

            EnsureStatusIsValid(task.Status);

            await _repository.AddAsync(task);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(string? priority = null)
        {
            return await _taskRepository.GetTasksAsync(priority);
        }

        public async Task UpdateTaskStatusAsync(long taskId, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ValidationException("El estado de la tarea es obligatorio");
            }

            EnsureStatusIsValid(status);

            var task = await _repository.FindAsync(taskId) ?? throw new NotFoundException($"No se encontró la tarea con Id: {taskId}");

            if (task.Status == TaskItem.PendingStatus && status == TaskItem.DoneStatus)
            {
                throw new ValidationException("No se permite cambiar una tarea directamente de Pending a Done");
            }

            task.Status = status;
            task.UpdatedAtUtc = DateTime.UtcNow;

            await _repository.UpdateAsync(task);
        }

        public async Task UpdateTaskPriorityAsync(long taskId, string priority)
        {
            if (string.IsNullOrWhiteSpace(priority))
            {
                throw new ValidationException("La prioridad de la tarea es obligatoria");
            }

            var task = await _repository.FindAsync(taskId) ?? throw new NotFoundException($"No se encontró la tarea con Id: {taskId}");

            task.AdditionalInfoJson = TaskAdditionalInfo.SetPriority(task.AdditionalInfoJson, priority);
            task.UpdatedAtUtc = DateTime.UtcNow;

            await _repository.UpdateAsync(task);
        }

        private static void EnsureStatusIsValid(string status)
        {
            if (!ValidStatuses.Contains(status))
            {
                throw new ValidationException($"Estado inválido: {status}. Estados permitidos: Pending, InProgress, Done");
            }
        }
    }
}
