using Application.Tasks.UpdateTaskPriority;
using MediatR;

namespace Domain.Test.Handler.UpdateTaskPriority
{
    public class UpdateTaskPriorityCommandTests
    {
        [Fact]
        public void UpdateTaskPriorityCommand_ShouldImplementIRequest()
        {
            var command = new UpdateTaskPriorityCommand(10, "Alta");

            Assert.IsAssignableFrom<IRequest>(command);
        }

        [Fact]
        public void UpdateTaskPriorityCommand_WithValidData_ShouldSetProperties()
        {
            var command = new UpdateTaskPriorityCommand(99, "Media");

            Assert.Equal(99, command.Id);
            Assert.Equal("Media", command.Priority);
        }

        [Fact]
        public void UpdateTaskPriorityCommand_WithSameValues_ShouldBeEqual()
        {
            var left = new UpdateTaskPriorityCommand(1, "Alta");
            var right = new UpdateTaskPriorityCommand(1, "Alta");

            Assert.Equal(left, right);
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
    }
}
