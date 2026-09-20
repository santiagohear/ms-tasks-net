using Application.Tasks.CreateTask;
using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using Application.Tasks.UpdateTaskPriority;
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
            var command = new CreateTaskCommand("Task", "Desc", 1, 2, null, null, "Media", ["frontend"]);

            await _controller.CreateAsync(command);

            await _mediator.Received(1).Send(Arg.Is<CreateTaskCommand>(c => c.Title == "Task" && c.Priority == "Media"), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldUseQueryPattern()
        {
            _mediator.Send(Arg.Any<GetTasksQuery>(), Arg.Any<CancellationToken>()).Returns(new List<TaskDto>());

            await _controller.GetAsync("Media");

            await _mediator.Received(1).Send(Arg.Is<GetTasksQuery>(q => q.Priority == "Media"), Arg.Any<CancellationToken>());
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

        [Fact]
        public async Task UpdatePriorityAsync_ShouldUseCommandPattern()
        {
            var request = new UpdateTaskPriorityRequest { Priority = "Alta" };

            await _controller.UpdatePriorityAsync(22, request);

            await _mediator.Received(1).Send(
                Arg.Is<UpdateTaskPriorityCommand>(c => c.Id == 22 && c.Priority == "Alta"),
                Arg.Any<CancellationToken>());
        }
    }
}
