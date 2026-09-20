using Application.Tasks.UpdateTaskStatus;
using MediatR;

namespace Domain.Test.Handler.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandTests
    {
        [Fact]
        public void UpdateTaskStatusCommand_ShouldImplementIRequest()
        {
            var command = new UpdateTaskStatusCommand(10, "InProgress");

            Assert.IsAssignableFrom<IRequest>(command);
        }

        [Fact]
        public void UpdateTaskStatusCommand_WithValidData_ShouldSetProperties()
        {
            var command = new UpdateTaskStatusCommand(99, "Done");

            Assert.Equal(99, command.Id);
            Assert.Equal("Done", command.Status);
        }

        [Fact]
        public void UpdateTaskStatusCommand_WithSameValues_ShouldBeEqual()
        {
            var left = new UpdateTaskStatusCommand(1, "Pending");
            var right = new UpdateTaskStatusCommand(1, "Pending");

            Assert.Equal(left, right);
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
    }
}
