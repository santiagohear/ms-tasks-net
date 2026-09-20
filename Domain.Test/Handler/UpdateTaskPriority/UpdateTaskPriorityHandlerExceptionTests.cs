using Application.Tasks.UpdateTaskPriority;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.UpdateTaskPriority
{
    public class UpdateTaskPriorityHandlerExceptionTests
    {
        private readonly ITaskService _taskService;
        private readonly UpdateTaskPriorityHandler _handler;

        public UpdateTaskPriorityHandlerExceptionTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _handler = new UpdateTaskPriorityHandler(_taskService);
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var command = new UpdateTaskPriorityCommand(10, "Alta");
            _taskService.UpdateTaskPriorityAsync(command.Id, command.Priority).ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
