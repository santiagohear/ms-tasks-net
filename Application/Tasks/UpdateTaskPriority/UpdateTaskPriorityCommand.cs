using MediatR;

namespace Application.Tasks.UpdateTaskPriority
{
    public record UpdateTaskPriorityCommand(
        long Id,
        string Priority
    ) : IRequest;
}
