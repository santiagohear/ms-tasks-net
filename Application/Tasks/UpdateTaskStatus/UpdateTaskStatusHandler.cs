using Domain.Ports.Services;
using MediatR;

namespace Application.Tasks.UpdateTaskStatus
{
    public class UpdateTaskStatusHandler(ITaskService service) : IRequestHandler<UpdateTaskStatusCommand>
    {
        private readonly ITaskService _service = service;

        public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            await _service.UpdateTaskStatusAsync(request.Id, request.Status);
        }
    }
}
