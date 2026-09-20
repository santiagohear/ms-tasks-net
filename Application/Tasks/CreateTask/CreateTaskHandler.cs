using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using MediatR;

namespace Application.Tasks.CreateTask
{
    public class CreateTaskHandler(ITaskService service, IMapper mapper) : IRequestHandler<CreateTaskCommand>
    {
        private readonly ITaskService _service = service;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await _service.CreateTaskAsync(_mapper.Map<TaskItem>(request));
        }
    }
}
