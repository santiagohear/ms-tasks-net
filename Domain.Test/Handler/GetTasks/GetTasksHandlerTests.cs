using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.GetTasks
{
    public class GetTasksHandlerTests
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly GetTasksHandler _handler;

        public GetTasksHandlerTests()
        {
            _taskService = Substitute.For<ITaskService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetTasksHandler(_taskService, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidQuery_ShouldReturnMappedDtos()
        {
            var query = new GetTasksQuery();
            var tasks = new List<TaskItem>
            {
                new() { Id = 1, Title = "Task 1", AssignedToUserId = 1, CreatedByUserId = 2, Status = TaskItem.PendingStatus }
            };
            var dtos = new List<TaskDto>
            {
                new() { Id = 1, Title = "Task 1", Status = TaskItem.PendingStatus, AssignedToUserId = 1, CreatedByUserId = 2 }
            };

            _taskService.GetTasksAsync().Returns(tasks);
            _mapper.Map<IEnumerable<TaskDto>>(tasks).Returns(dtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            _mapper.Received(1).Map<IEnumerable<TaskDto>>(tasks);
            await _taskService.Received(1).GetTasksAsync();
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyCollection()
        {
            var query = new GetTasksQuery();
            var tasks = Array.Empty<TaskItem>();
            var dtos = Array.Empty<TaskDto>();

            _taskService.GetTasksAsync().Returns(tasks);
            _mapper.Map<IEnumerable<TaskDto>>(tasks).Returns(dtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
