using Application.Users.CreateUser;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.CreateUser
{
    public class CreateUserHandlerExceptionTests
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerExceptionTests()
        {
            _userService = Substitute.For<IUserService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateUserHandler(_userService, _mapper);
        }

        [Fact]
        public async Task Handle_WhenMapperThrowsException_ShouldPropagateException()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");
            _mapper.Map<User>(command).Throws(new AutoMapperMappingException("Mapping failed"));

            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(command, CancellationToken.None));
            await _userService.DidNotReceive().CreateUserAsync(Arg.Any<User?>());
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");
            var user = new User { UserName = command.UserName, Email = command.Email };

            _mapper.Map<User>(command).Returns(user);
            _userService.CreateUserAsync(user).ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
