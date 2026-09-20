using Application.Tasks.CreateTask;
using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using Application.Tasks.UpdateTaskPriority;
using Application.Tasks.UpdateTaskStatus;
using Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Tasks;

namespace WebApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateAsync([Validate] CreateTaskCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<TaskDto>> GetAsync([FromQuery] string? priority = null)
        {
            return await _mediator.Send(new GetTasksQuery(priority));
        }

        [HttpPut("{id:long}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateStatusAsync(long id, [Validate] UpdateTaskStatusRequest request)
        {
            await _mediator.Send(new UpdateTaskStatusCommand(id, request.Status));
            return Ok();
        }

        [HttpPut("{id:long}/priority")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdatePriorityAsync(long id, [Validate] UpdateTaskPriorityRequest request)
        {
            await _mediator.Send(new UpdateTaskPriorityCommand(id, request.Priority));
            return Ok();
        }
    }
}
