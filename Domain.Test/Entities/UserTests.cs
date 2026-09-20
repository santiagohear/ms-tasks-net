using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Entities.Base;

namespace Domain.Test.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldInheritFromBaseEntityInt()
        {
            var user = new User();

            Assert.IsAssignableFrom<BaseEntity<int>>(user);
        }

        [Fact]
        public void DefaultValues_ShouldBeInitializedCorrectly()
        {
            var before = DateTime.UtcNow;
            var user = new User();
            var after = DateTime.UtcNow;

            Assert.True(user.IsActive);
            Assert.True(user.CreatedAtUtc >= before && user.CreatedAtUtc <= after);
            Assert.NotNull(user.AssignedTasks);
            Assert.NotNull(user.CreatedTasks);
            Assert.Empty(user.AssignedTasks);
            Assert.Empty(user.CreatedTasks);
        }

        [Fact]
        public void Properties_WithObjectInitializer_ShouldSetValues()
        {
            var assignedTask = new TaskItem { Id = 1, Title = "Assigned task", AssignedToUserId = 7, CreatedByUserId = 8 };
            var createdTask = new TaskItem { Id = 2, Title = "Created task", AssignedToUserId = 8, CreatedByUserId = 7 };

            var user = new User
            {
                Id = 7,
                UserName = "Santi",
                Email = "santi@test.com",
                IsActive = false,
                AssignedTasks = new List<TaskItem> { assignedTask },
                CreatedTasks = new List<TaskItem> { createdTask }
            };

            Assert.Equal(7, user.Id);
            Assert.Equal("Santi", user.UserName);
            Assert.Equal("santi@test.com", user.Email);
            Assert.False(user.IsActive);
            Assert.Single(user.AssignedTasks);
            Assert.Single(user.CreatedTasks);
            Assert.Equal(assignedTask, user.AssignedTasks.First());
            Assert.Equal(createdTask, user.CreatedTasks.First());
        }

        [Fact]
        public void MaxLengthAttributes_ShouldBeConfiguredCorrectly()
        {
            var userNameAttr = typeof(User).GetProperty(nameof(User.UserName))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).SingleOrDefault() as MaxLengthAttribute;
            var emailAttr = typeof(User).GetProperty(nameof(User.Email))?.GetCustomAttributes(typeof(MaxLengthAttribute), false).SingleOrDefault() as MaxLengthAttribute;

            Assert.NotNull(userNameAttr);
            Assert.Equal(100, userNameAttr!.Length);
            Assert.NotNull(emailAttr);
            Assert.Equal(255, emailAttr!.Length);
        }

        [Fact]
        public void TaskCollections_ShouldAllowAddingItems()
        {
            var user = new User { Id = 1, UserName = "User", Email = "user@test.com" };
            var task = new TaskItem { Id = 10, Title = "Task", AssignedToUserId = 1, CreatedByUserId = 1 };

            user.AssignedTasks.Add(task);
            user.CreatedTasks.Add(task);

            Assert.Contains(task, user.AssignedTasks);
            Assert.Contains(task, user.CreatedTasks);
        }
    }
}
