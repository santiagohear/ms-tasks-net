using Application.Users.CreateUser;
using Application.Users.GetUsers;
using Application.Users.Shared;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WebApi.Controllers;

namespace WebApi.Integration.Test.Controllers
{
    public class UsersControllerExceptionTests
    {
        private readonly IMediator _mediator;
        private readonly UsersController _controller;

        public UsersControllerExceptionTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new UsersController(_mediator);
        }

        [Fact]
        public async Task CreateAsync_WhenMediatorThrows_ShouldPropagateException()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");
            _mediator.Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new InvalidOperationException("Mediator error"));

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.CreateAsync(command));

            Assert.Equal("Mediator error", ex.Message);
        }

        [Fact]
        public async Task GetAsync_WhenMediatorThrows_ShouldPropagateException()
        {
            _mediator.Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new TimeoutException("Timeout"));

            var ex = await Assert.ThrowsAsync<TimeoutException>(() => _controller.GetAsync());

            Assert.Equal("Timeout", ex.Message);
        }
    }
}
