using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Services;
using NSubstitute;

namespace Domain.Test.Services
{
    public class TaskServiceTests
    {
        private readonly ITaskRepository _taskRepositoryMock;
        private readonly IRepository<TaskItem> _taskItemRepositoryMock;
        private readonly IRepository<User> _userRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = Substitute.For<ITaskRepository>();
            _taskItemRepositoryMock = Substitute.For<IRepository<TaskItem>>();
            _userRepositoryMock = Substitute.For<IRepository<User>>();
            _taskService = new TaskService(_taskRepositoryMock, _taskItemRepositoryMock, _userRepositoryMock);
        }

        [Fact]
        public async Task CreateTaskAsync_WithValidTask_ShouldAddTask()
        {
            var task = new TaskItem
            {
                Id = 10,
                Title = "Task 1",
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Status = TaskItem.PendingStatus
            };

            _userRepositoryMock.FindAsync(task.AssignedToUserId).Returns(new User { Id = task.AssignedToUserId, UserName = "Assigned", Email = "assigned@test.com" });
            _userRepositoryMock.FindAsync(task.CreatedByUserId).Returns(new User { Id = task.CreatedByUserId, UserName = "Creator", Email = "creator@test.com" });

            await _taskService.CreateTaskAsync(task);

            await _taskItemRepositoryMock.Received(1).AddAsync(task);
        }

        [Fact]
        public async Task CreateTaskAsync_WithEmptyStatus_ShouldAssignPendingStatusAndAddTask()
        {
            var task = new TaskItem
            {
                Id = 11,
                Title = "Task 2",
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Status = string.Empty
            };

            _userRepositoryMock.FindAsync(task.AssignedToUserId).Returns(new User { Id = task.AssignedToUserId, UserName = "Assigned", Email = "assigned@test.com" });
            _userRepositoryMock.FindAsync(task.CreatedByUserId).Returns(new User { Id = task.CreatedByUserId, UserName = "Creator", Email = "creator@test.com" });

            await _taskService.CreateTaskAsync(task);

            Assert.Equal(TaskItem.PendingStatus, task.Status);
            await _taskItemRepositoryMock.Received(1).AddAsync(task);
        }

        [Fact]
        public async Task GetTasksAsync_ShouldReturnTasksFromRepository()
        {
            var existingTask = new TaskItem
            {
                Id = 10,
                Title = "Task 1",
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Status = TaskItem.PendingStatus
            };
            var tasks = new[] { existingTask };

            _taskRepositoryMock.GetTasksAsync().Returns(tasks);

            var result = await _taskService.GetTasksAsync();

            Assert.Single(result);
            Assert.Equal(existingTask, result.Single());
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_WithValidTransition_ShouldUpdateStatusAndTimestamp()
        {
            var task = new TaskItem
            {
                Id = 10,
                Title = "Task 1",
                AssignedToUserId = 1,
                CreatedByUserId = 2,
                Status = TaskItem.PendingStatus
            };

            _taskItemRepositoryMock.FindAsync(task.Id).Returns(task);

            await _taskService.UpdateTaskStatusAsync(task.Id, TaskItem.InProgressStatus);

            Assert.Equal(TaskItem.InProgressStatus, task.Status);
            Assert.NotNull(task.UpdatedAtUtc);
            await _taskItemRepositoryMock.Received(1).UpdateAsync(task);
        }
    }
}
