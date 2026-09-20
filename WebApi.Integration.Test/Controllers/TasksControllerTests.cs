using Application.Tasks.CreateTask;
using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using Application.Tasks.UpdateTaskPriority;
using Application.Tasks.UpdateTaskStatus;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebApi.Controllers;
using WebApi.Contracts.Tasks;

namespace WebApi.Integration.Test.Controllers
{
    public class TasksControllerTests
    {
        private readonly IMediator _mediator;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new TasksController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_WithValidCommand_ShouldReturnCreatedAndSendCommand()
        {
            var command = new CreateTaskCommand("Task", "Desc", 1, 2, null, null, "Media", ["frontend", "ux"]);

            var result = await _controller.CreateAsync(command);

            var status = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, status.StatusCode);
            await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnTasksFromMediator()
        {
            var tasks = new List<TaskDto>
            {
                new() { Id = 1, Title = "Task 1", Status = "Pending", AssignedToUserId = 1, CreatedByUserId = 2, Priority = "Media", Tags = ["frontend", "ux"] }
            };
            _mediator.Send(Arg.Any<GetTasksQuery>(), Arg.Any<CancellationToken>()).Returns(tasks);

            var result = await _controller.GetAsync("Media");

            Assert.Single(result);
            await _mediator.Received(1).Send(Arg.Is<GetTasksQuery>(q => q.Priority == "Media"), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateStatusAsync_WithValidRequest_ShouldReturnOkAndSendCommand()
        {
            var request = new UpdateTaskStatusRequest { Status = "InProgress" };

            var result = await _controller.UpdateStatusAsync(10, request);

            Assert.IsType<OkResult>(result);
            await _mediator.Received(1).Send(
                Arg.Is<UpdateTaskStatusCommand>(c => c.Id == 10 && c.Status == "InProgress"),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdatePriorityAsync_WithValidRequest_ShouldReturnOkAndSendCommand()
        {
            var request = new UpdateTaskPriorityRequest { Priority = "Alta" };

            var result = await _controller.UpdatePriorityAsync(10, request);

            Assert.IsType<OkResult>(result);
            await _mediator.Received(1).Send(
                Arg.Is<UpdateTaskPriorityCommand>(c => c.Id == 10 && c.Priority == "Alta"),
                Arg.Any<CancellationToken>());
        }
    }
}
