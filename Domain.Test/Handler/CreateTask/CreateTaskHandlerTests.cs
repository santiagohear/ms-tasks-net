using Application.Tasks.CreateTask;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.CreateTask
{
    public class CreateTaskHandlerTests
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly CreateTaskHandler _handler;

        public CreateTaskHandlerTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateTaskHandler(_taskService, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldMapAndCreateTask()
        {
            var command = new CreateTaskCommand("Task 1", "Description", 1, 2, null, null);
            var taskEntity = new TaskItem
            {
                Id = 10,
                Title = command.Title,
                Description = command.Description,
                AssignedToUserId = command.AssignedToUserId,
                CreatedByUserId = command.CreatedByUserId
            };

            _mapper.Map<TaskItem>(command).Returns(taskEntity);

            await _handler.Handle(command, CancellationToken.None);

            _mapper.Received(1).Map<TaskItem>(command);
            await _taskService.Received(1).CreateTaskAsync(taskEntity);
        }

        [Fact]
        public async Task Handle_WhenMapperReturnsNull_ShouldCallServiceWithNull()
        {
            var command = new CreateTaskCommand("Task 1", null, 1, 2, null, null);
            _mapper.Map<TaskItem>(command).Returns((TaskItem?)null);

            await _handler.Handle(command, CancellationToken.None);

            await _taskService.Received(1).CreateTaskAsync(null);
        }
    }
}
