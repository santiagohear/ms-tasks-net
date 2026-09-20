using Application.Users.CreateUser;
using Application.Users.GetUsers;
using Application.Users.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebApi.Controllers;

namespace WebApi.Integration.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly IMediator _mediator;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new UsersController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_WithValidCommand_ShouldReturnCreatedAndSendCommand()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");

            var result = await _controller.CreateAsync(command);

            var status = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, status.StatusCode);
            await _mediator.Received(1).Send(command, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnUsersFromMediator()
        {
            var users = new List<UserDto>
            {
                new() { Id = 1, UserName = "A", Email = "a@test.com" },
                new() { Id = 2, UserName = "B", Email = "b@test.com" }
            };
            _mediator.Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>()).Returns(users);

            var result = await _controller.GetAsync();

            Assert.Equal(2, result.Count());
            await _mediator.Received(1).Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>());
        }
    }
}
