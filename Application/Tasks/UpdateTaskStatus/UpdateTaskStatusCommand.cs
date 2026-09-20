using MediatR;

namespace Application.Tasks.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(
        long Id,
        string Status
    ) : IRequest;
}
