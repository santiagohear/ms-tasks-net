using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Entities.Base;

namespace Domain.Test.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void TaskItem_ShouldInheritFromBaseEntityLong()
        {
            var taskItem = new TaskItem();

            Assert.IsAssignableFrom<BaseEntity<long>>(taskItem);
        }

        [Fact]
        public void StatusConstants_ShouldMatchExpectedValues()
        {
            Assert.Equal("Pending", TaskItem.PendingStatus);
            Assert.Equal("InProgress", TaskItem.InProgressStatus);
            Assert.Equal("Done", TaskItem.DoneStatus);
        }

        [Fact]
        public void DefaultValues_ShouldBeInitializedCorrectly()
        {
            var before = DateTime.UtcNow;
            var taskItem = new TaskItem();
            var after = DateTime.UtcNow;

            Assert.Equal(TaskItem.PendingStatus, taskItem.Status);
            Assert.True(taskItem.CreatedAtUtc >= before && taskItem.CreatedAtUtc <= after);
            Assert.Null(taskItem.UpdatedAtUtc);
            Assert.Null(taskItem.EstimatedFinishDate);
            Assert.Null(taskItem.Description);
            Assert.Null(taskItem.AdditionalInfoJson);
        }

        [Fact]
        public void Properties_WithObjectInitializer_ShouldSetValues()
        {
            var assignedUser = new User { Id = 10, UserName = "Assigned", Email = "assigned@test.com" };
            var createdByUser = new User { Id = 20, UserName = "Creator", Email = "creator@test.com" };
            var estimatedDate = new DateTime(2030, 1, 1);
            var updatedAt = DateTime.UtcNow;

            var taskItem = new TaskItem
            {
                Id = 99,
                Title = "Task title",
                Description = "Task description",
                Status = TaskItem.InProgressStatus,
                AssignedToUserId = 10,
                AssignedToUser = assignedUser,
                CreatedByUserId = 20,
                CreatedByUser = createdByUser,
                EstimatedFinishDate = estimatedDate,
                UpdatedAtUtc = updatedAt,
                AdditionalInfoJson = "{\"priority\":\"high\"}"
            };

            Assert.Equal(99, taskItem.Id);
            Assert.Equal("Task title", taskItem.Title);
            Assert.Equal("Task description", taskItem.Description);
            Assert.Equal(TaskItem.InProgressStatus, taskItem.Status);
            Assert.Equal(10, taskItem.AssignedToUserId);
            Assert.Equal(assignedUser, taskItem.AssignedToUser);
            Assert.Equal(20, taskItem.CreatedByUserId);
            Assert.Equal(createdByUser, taskItem.CreatedByUser);
            Assert.Equal(estimatedDate, taskItem.EstimatedFinishDate);
            Assert.Equal(updatedAt, taskItem.UpdatedAtUtc);
            Assert.Equal("{\"priority\":\"high\"}", taskItem.AdditionalInfoJson);
        }

        [Fact]
        public void MaxLengthAttributes_ShouldBeConfiguredCorrectly()
        {
            var titleAttr = typeof(TaskItem).GetProperty(nameof(TaskItem.Title))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).SingleOrDefault() as MaxLengthAttribute;
            var descriptionAttr = typeof(TaskItem).GetProperty(nameof(TaskItem.Description))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).SingleOrDefault() as MaxLengthAttribute;
            var statusAttr = typeof(TaskItem).GetProperty(nameof(TaskItem.Status))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).SingleOrDefault() as MaxLengthAttribute;

            Assert.NotNull(titleAttr);
            Assert.Equal(200, titleAttr!.Length);
            Assert.NotNull(descriptionAttr);
            Assert.Equal(2000, descriptionAttr!.Length);
            Assert.NotNull(statusAttr);
            Assert.Equal(30, statusAttr!.Length);
        }
    }
}
