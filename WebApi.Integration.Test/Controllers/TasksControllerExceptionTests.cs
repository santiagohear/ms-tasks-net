using Application.Tasks.CreateTask;
using Application.Tasks.GetTasks;
using Application.Tasks.UpdateTaskStatus;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WebApi.Controllers;
using WebApi.Contracts.Tasks;

namespace WebApi.Integration.Test.Controllers
{
    public class TasksControllerExceptionTests
    {
        private readonly IMediator _mediator;
        private readonly TasksController _controller;

        public TasksControllerExceptionTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new TasksController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_WhenMediatorThrows_ShouldPropagateException()
        {
            var command = new CreateTaskCommand("Task", null, 1, 2, null, null);
            _mediator.Send(Arg.Any<CreateTaskCommand>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new InvalidOperationException("Mediator error"));

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.CreateAsync(command));

            Assert.Equal("Mediator error", ex.Message);
        }

        [Fact]
        public async Task GetAsync_WhenMediatorThrows_ShouldPropagateException()
        {
            _mediator.Send(Arg.Any<GetTasksQuery>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new TimeoutException("Timeout"));

            var ex = await Assert.ThrowsAsync<TimeoutException>(() => _controller.GetAsync());

            Assert.Equal("Timeout", ex.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenMediatorThrows_ShouldPropagateException()
        {
            _mediator.Send(Arg.Any<UpdateTaskStatusCommand>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new ArgumentException("Invalid status"));

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _controller.UpdateStatusAsync(1, new UpdateTaskStatusRequest { Status = "Invalid" }));

            Assert.Equal("Invalid status", ex.Message);
        }
    }
}
