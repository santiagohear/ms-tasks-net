using Application.Tasks.UpdateTaskStatus;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.UpdateTaskStatus
{
    public class UpdateTaskStatusHandlerExceptionTests
    {
        private readonly ITaskService _taskService;
        private readonly UpdateTaskStatusHandler _handler;

        public UpdateTaskStatusHandlerExceptionTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _handler = new UpdateTaskStatusHandler(_taskService);
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var command = new UpdateTaskStatusCommand(10, "Done");
            _taskService.UpdateTaskStatusAsync(command.Id, command.Status)
                .ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsValidationException_ShouldPropagateException()
        {
            var command = new UpdateTaskStatusCommand(10, "InvalidStatus");
            _taskService.UpdateTaskStatusAsync(command.Id, command.Status)
                .ThrowsAsync(new ArgumentException("Invalid status"));

            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
