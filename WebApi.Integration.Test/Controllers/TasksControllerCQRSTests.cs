using Application.Tasks.CreateTask;
using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using Application.Tasks.UpdateTaskStatus;
using MediatR;
using NSubstitute;
using WebApi.Controllers;
using WebApi.Contracts.Tasks;

namespace WebApi.Integration.Test.Controllers
{
    public class TasksControllerCQRSTests
    {
        private readonly IMediator _mediator;
        private readonly TasksController _controller;

        public TasksControllerCQRSTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new TasksController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_ShouldUseCommandPattern()
        {
            var command = new CreateTaskCommand("Task", "Desc", 1, 2, null, null);

            await _controller.CreateAsync(command);

            await _mediator.Received(1).Send(Arg.Is<CreateTaskCommand>(c => c.Title == "Task"), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldUseQueryPattern()
        {
            _mediator.Send(Arg.Any<GetTasksQuery>(), Arg.Any<CancellationToken>()).Returns(new List<TaskDto>());

            await _controller.GetAsync();

            await _mediator.Received(1).Send(Arg.Any<GetTasksQuery>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUseCommandPattern()
        {
            var request = new UpdateTaskStatusRequest { Status = "Done" };

            await _controller.UpdateStatusAsync(22, request);

            await _mediator.Received(1).Send(
                Arg.Is<UpdateTaskStatusCommand>(c => c.Id == 22 && c.Status == "Done"),
                Arg.Any<CancellationToken>());
        }
    }
}
