using Application.Tasks.Shared;
using MediatR;

namespace Application.Tasks.GetTasks
{
    public record GetTasksQuery() : IRequest<IEnumerable<TaskDto>>;
}
