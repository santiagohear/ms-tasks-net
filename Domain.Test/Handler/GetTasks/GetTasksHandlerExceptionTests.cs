using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.GetTasks
{
    public class GetTasksHandlerExceptionTests
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly GetTasksHandler _handler;

        public GetTasksHandlerExceptionTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetTasksHandler(_taskService, _mapper);
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var query = new GetTasksQuery("Alta");
            _taskService.GetTasksAsync(query.Priority).ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
            _mapper.DidNotReceive().Map<IEnumerable<TaskDto>>(Arg.Any<IEnumerable<TaskItem>>());
        }

        [Fact]
        public async Task Handle_WhenMapperThrowsException_ShouldPropagateException()
        {
            var query = new GetTasksQuery();
            var tasks = new List<TaskItem>
            {
                new() { Id = 1, Title = "Task 1", AssignedToUserId = 1, CreatedByUserId = 2, Status = TaskItem.PendingStatus }
            };

            _taskService.GetTasksAsync(query.Priority).Returns(tasks);
            _mapper.Map<IEnumerable<TaskDto>>(tasks).Throws(new AutoMapperMappingException("Mapping failed"));

            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
