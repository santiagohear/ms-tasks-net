using Application.Tasks.UpdateTaskPriority;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.UpdateTaskPriority
{
    public class UpdateTaskPriorityHandlerTests
    {
        private readonly ITaskService _taskService;
        private readonly UpdateTaskPriorityHandler _handler;

        public UpdateTaskPriorityHandlerTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _handler = new UpdateTaskPriorityHandler(_taskService);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCallService()
        {
            var command = new UpdateTaskPriorityCommand(10, "Alta");

            await _handler.Handle(command, CancellationToken.None);

            await _taskService.Received(1).UpdateTaskPriorityAsync(command.Id, command.Priority);
        }
    }
}
