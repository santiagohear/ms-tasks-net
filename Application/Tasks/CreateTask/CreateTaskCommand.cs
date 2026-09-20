using MediatR;

namespace Application.Tasks.CreateTask
{
    public record CreateTaskCommand(
        string Title,
        string? Description,
        int AssignedToUserId,
        int CreatedByUserId,
        DateTime? EstimatedFinishDate,
        string? AdditionalInfoJson
    ) : IRequest;
}
