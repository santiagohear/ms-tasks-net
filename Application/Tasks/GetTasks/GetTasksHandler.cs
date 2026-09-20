using Application.Tasks.Shared;
using AutoMapper;
using Domain.Ports.Services;
using MediatR;

namespace Application.Tasks.GetTasks
{
    public class GetTasksHandler(ITaskService service, IMapper mapper) : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskService _service = service;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _service.GetTasksAsync(request.Priority);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }
    }
}
