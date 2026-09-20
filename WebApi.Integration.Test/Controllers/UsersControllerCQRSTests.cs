using Application.Users.CreateUser;
using Application.Users.GetUsers;
using Application.Users.Shared;
using MediatR;
using NSubstitute;
using WebApi.Controllers;

namespace WebApi.Integration.Test.Controllers
{
    public class UsersControllerCQRSTests
    {
        private readonly IMediator _mediator;
        private readonly UsersController _controller;

        public UsersControllerCQRSTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new UsersController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_ShouldUseCommandPattern()
        {
            var command = new CreateUserCommand("User", "user@test.com");

            await _controller.CreateAsync(command);

            await _mediator.Received(1).Send(
                Arg.Is<CreateUserCommand>(c => c.UserName == "User" && c.Email == "user@test.com"),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldUseQueryPattern()
        {
            var users = new List<UserDto>();
            _mediator.Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>()).Returns(users);

            await _controller.GetAsync();

            await _mediator.Received(1).Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>());
        }
    }
}
