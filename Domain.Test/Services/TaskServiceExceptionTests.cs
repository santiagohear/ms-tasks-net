using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports.Repositories;
using Domain.Services;
using NSubstitute;

namespace Domain.Test.Services
{
    public class TaskServiceExceptionTests
    {
        private readonly ITaskRepository _taskRepositoryMock;
        private readonly IRepository<TaskItem> _taskItemRepositoryMock;
        private readonly IRepository<User> _userRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceExceptionTests()
        {
            _taskRepositoryMock = Substitute.For<ITaskRepository>();
            _taskItemRepositoryMock = Substitute.For<IRepository<TaskItem>>();
            _userRepositoryMock = Substitute.For<IRepository<User>>();
            _taskService = new TaskService(_taskRepositoryMock, _taskItemRepositoryMock, _userRepositoryMock);
        }

        [Fact]
        public async Task CreateTaskAsync_WithNullTask_ShouldThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.CreateTaskAsync(null));
        }

        [Fact]
        public async Task CreateTaskAsync_WithEmptyTitle_ShouldThrowValidationException()
        {
            var task = new TaskItem { AssignedToUserId = 1, CreatedByUserId = 2, Title = string.Empty };

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task CreateTaskAsync_WithInvalidJson_ShouldThrowValidationException()
        {
            var task = new TaskItem
            {
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Title = "Task",
                AdditionalInfoJson = "{invalid-json}"
            };

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task CreateTaskAsync_WhenAssignedUserDoesNotExist_ShouldThrowValidationException()
        {
            var task = new TaskItem { AssignedToUserId = 999, CreatedByUserId = 2, Title = "Task" };

            _userRepositoryMock.FindAsync(task.AssignedToUserId).Returns((User?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task CreateTaskAsync_WhenCreatedByUserDoesNotExist_ShouldThrowValidationException()
        {
            var task = new TaskItem { AssignedToUserId = 1, CreatedByUserId = 999, Title = "Task" };

            _userRepositoryMock.FindAsync(task.AssignedToUserId).Returns(new User { Id = task.AssignedToUserId, UserName = "Assigned", Email = "assigned@test.com" });
            _userRepositoryMock.FindAsync(task.CreatedByUserId).Returns((User?)null);

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task CreateTaskAsync_WithInvalidStatus_ShouldThrowValidationException()
        {
            var task = new TaskItem
            {
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Title = "Task",
                Status = "InvalidStatus"
            };

            _userRepositoryMock.FindAsync(task.AssignedToUserId).Returns(new User { Id = task.AssignedToUserId, UserName = "Assigned", Email = "assigned@test.com" });
            _userRepositoryMock.FindAsync(task.CreatedByUserId).Returns(new User { Id = task.CreatedByUserId, UserName = "Creator", Email = "creator@test.com" });

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.CreateTaskAsync(task));
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_WithEmptyStatus_ShouldThrowValidationException()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _taskService.UpdateTaskStatusAsync(1, string.Empty));
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_WithInvalidStatus_ShouldThrowValidationException()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _taskService.UpdateTaskStatusAsync(1, "InvalidStatus"));
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
        {
            _taskItemRepositoryMock.FindAsync(404L).Returns((TaskItem?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _taskService.UpdateTaskStatusAsync(404, TaskItem.InProgressStatus));
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_WhenTransitionIsPendingToDone_ShouldThrowValidationException()
        {
            var task = new TaskItem
            {
                Id = 1,
                Title = "Task",
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Status = TaskItem.PendingStatus
            };

            _taskItemRepositoryMock.FindAsync(task.Id).Returns(task);

            await Assert.ThrowsAsync<ValidationException>(() => _taskService.UpdateTaskStatusAsync(task.Id, TaskItem.DoneStatus));
        }

        [Fact]
        public async Task UpdateTaskPriorityAsync_WithEmptyPriority_ShouldThrowValidationException()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _taskService.UpdateTaskPriorityAsync(1, string.Empty));
        }

        [Fact]
        public async Task UpdateTaskPriorityAsync_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
        {
            _taskItemRepositoryMock.FindAsync(404L).Returns((TaskItem?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _taskService.UpdateTaskPriorityAsync(404, "Alta"));
        }
    }
}
