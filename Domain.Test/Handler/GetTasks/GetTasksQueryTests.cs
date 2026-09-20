using Application.Tasks.GetTasks;
using Application.Tasks.Shared;
using MediatR;

namespace Domain.Test.Handler.GetTasks
{
    public class GetTasksQueryTests
    {
        [Fact]
        public void GetTasksQuery_ShouldImplementIRequestOfTaskDtoCollection()
        {
            var query = new GetTasksQuery();

            Assert.IsAssignableFrom<IRequest<IEnumerable<TaskDto>>>(query);
        }

        [Fact]
        public void GetTasksQuery_WithPriority_ShouldSetProperty()
        {
            var query = new GetTasksQuery("Media");

            Assert.Equal("Media", query.Priority);
        }

        [Fact]
        public void GetTasksQuery_ToString_ShouldContainTypeName()
        {
            var query = new GetTasksQuery();

            Assert.Contains("GetTasksQuery", query.ToString());
        }
    }
}
