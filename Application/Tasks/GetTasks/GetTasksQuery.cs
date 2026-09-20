using Application.Tasks.Shared;
using MediatR;

namespace Application.Tasks.GetTasks
{
    public record GetTasksQuery(string? Priority = null) : IRequest<IEnumerable<TaskDto>>;
}
