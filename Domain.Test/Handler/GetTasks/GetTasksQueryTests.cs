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
        public void GetTasksQuery_MultipleInstances_ShouldBeEqual()
        {
            var query1 = new GetTasksQuery();
            var query2 = new GetTasksQuery();

            Assert.Equal(query1, query2);
            Assert.Equal(query1.GetHashCode(), query2.GetHashCode());
        }

        [Fact]
        public void GetTasksQuery_ToString_ShouldContainTypeName()
        {
            var query = new GetTasksQuery();

            Assert.Contains("GetTasksQuery", query.ToString());
        }
    }
}
