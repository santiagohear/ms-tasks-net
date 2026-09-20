using Application.Tasks.UpdateTaskStatus;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.UpdateTaskStatus
{
    public class UpdateTaskStatusHandlerTests
    {
        private readonly ITaskService _taskService;
        private readonly UpdateTaskStatusHandler _handler;

        public UpdateTaskStatusHandlerTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _handler = new UpdateTaskStatusHandler(_taskService);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCallService()
        {
            var command = new UpdateTaskStatusCommand(10, "InProgress");

            await _handler.Handle(command, CancellationToken.None);

            await _taskService.Received(1).UpdateTaskStatusAsync(command.Id, command.Status);
        }

        [Fact]
        public async Task Handle_WithDifferentStatuses_ShouldForwardValues()
        {
            var command = new UpdateTaskStatusCommand(20, "Done");

            await _handler.Handle(command, CancellationToken.None);

            await _taskService.Received(1).UpdateTaskStatusAsync(20, "Done");
        }
    }
}
