using MediatR;

namespace Application.Tasks.CreateTask
{
    public record CreateTaskCommand(
        string Title,
        string? Description,
        int AssignedToUserId,
        int CreatedByUserId,
        DateTime? EstimatedFinishDate,
        string? AdditionalInfoJson,
        string? Priority = null,
        string[]? Tags = null
    ) : IRequest;
}
