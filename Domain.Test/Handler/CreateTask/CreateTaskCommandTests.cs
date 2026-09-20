using Application.Tasks.CreateTask;
using MediatR;

namespace Domain.Test.Handler.CreateTask
{
    public class CreateTaskCommandTests
    {
        [Fact]
        public void CreateTaskCommand_ShouldImplementIRequest()
        {
            var command = new CreateTaskCommand("Task 1", "Description", 1, 2, DateTime.UtcNow, "{}");

            Assert.IsAssignableFrom<IRequest>(command);
        }

        [Fact]
        public void CreateTaskCommand_WithValidData_ShouldSetProperties()
        {
            var estimatedDate = new DateTime(2030, 1, 1);

            var command = new CreateTaskCommand("Task 1", "Description", 10, 20, estimatedDate, "{\"a\":1}");

            Assert.Equal("Task 1", command.Title);
            Assert.Equal("Description", command.Description);
            Assert.Equal(10, command.AssignedToUserId);
            Assert.Equal(20, command.CreatedByUserId);
            Assert.Equal(estimatedDate, command.EstimatedFinishDate);
            Assert.Equal("{\"a\":1}", command.AdditionalInfoJson);
        }

        [Fact]
        public void CreateTaskCommand_WithNullOptionalValues_ShouldSetNulls()
        {
            var command = new CreateTaskCommand("Task 1", null, 1, 2, null, null);

            Assert.Null(command.Description);
            Assert.Null(command.EstimatedFinishDate);
            Assert.Null(command.AdditionalInfoJson);
        }

        [Fact]
        public void CreateTaskCommand_WithSameValues_ShouldBeEqual()
        {
            var left = new CreateTaskCommand("Task", "Desc", 1, 2, null, null);
            var right = new CreateTaskCommand("Task", "Desc", 1, 2, null, null);

            Assert.Equal(left, right);
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
    }
}
