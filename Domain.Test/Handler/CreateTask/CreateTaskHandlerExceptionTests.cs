using Application.Tasks.CreateTask;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.CreateTask
{
    public class CreateTaskHandlerExceptionTests
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly CreateTaskHandler _handler;

        public CreateTaskHandlerExceptionTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateTaskHandler(_taskService, _mapper);
        }

        [Fact]
        public async Task Handle_WhenMapperThrowsException_ShouldPropagateException()
        {
            var command = new CreateTaskCommand("Task 1", null, 1, 2, null, null);
            _mapper.Map<TaskItem>(command).Throws(new AutoMapperMappingException("Mapping failed"));

            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(command, CancellationToken.None));
            await _taskService.DidNotReceive().CreateTaskAsync(Arg.Any<TaskItem?>());
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var command = new CreateTaskCommand("Task 1", null, 1, 2, null, null);
            var taskEntity = new TaskItem { Title = command.Title, AssignedToUserId = 1, CreatedByUserId = 2 };

            _mapper.Map<TaskItem>(command).Returns(taskEntity);
            _taskService.CreateTaskAsync(taskEntity).ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
