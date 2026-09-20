using Application.Users.CreateUser;
using MediatR;

namespace Domain.Test.Handler.CreateUser
{
    public class CreateUserCommandTests
    {
        [Fact]
        public void CreateUserCommand_ShouldImplementIRequest()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");

            Assert.IsAssignableFrom<IRequest>(command);
        }

        [Fact]
        public void CreateUserCommand_WithValidData_ShouldSetProperties()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");

            Assert.Equal("Santi", command.UserName);
            Assert.Equal("santi@test.com", command.Email);
        }

        [Theory]
        [InlineData("", "mail@test.com")]
        [InlineData("User", "")]
        [InlineData(null, "mail@test.com")]
        [InlineData("User", null)]
        public void CreateUserCommand_WithEmptyOrNullValues_ShouldStillCreateRecord(string? userName, string? email)
        {
            var command = new CreateUserCommand(userName!, email!);

            Assert.Equal(userName, command.UserName);
            Assert.Equal(email, command.Email);
        }

        [Fact]
        public void CreateUserCommand_WithSameValues_ShouldBeEqual()
        {
            var left = new CreateUserCommand("Santi", "santi@test.com");
            var right = new CreateUserCommand("Santi", "santi@test.com");

            Assert.Equal(left, right);
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
    }
}
