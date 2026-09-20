using Domain.Ports.Services;
using MediatR;

namespace Application.Tasks.UpdateTaskPriority
{
    public class UpdateTaskPriorityHandler(ITaskService service) : IRequestHandler<UpdateTaskPriorityCommand>
    {
        private readonly ITaskService _service = service;

        public async Task Handle(UpdateTaskPriorityCommand request, CancellationToken cancellationToken)
        {
            await _service.UpdateTaskPriorityAsync(request.Id, request.Priority);
        }
    }
}
