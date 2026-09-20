using Application.Users.CreateUser;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.CreateUser
{
    public class CreateUserHandlerTests
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _userService = Substitute.For<IUserService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateUserHandler(_userService, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldMapAndCreateUser()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");
            var user = new User { Id = 1, UserName = command.UserName, Email = command.Email };

            _mapper.Map<User>(command).Returns(user);

            await _handler.Handle(command, CancellationToken.None);

            _mapper.Received(1).Map<User>(command);
            await _userService.Received(1).CreateUserAsync(user);
        }

        [Fact]
        public async Task Handle_WhenMapperReturnsNull_ShouldCallServiceWithNull()
        {
            var command = new CreateUserCommand("Santi", "santi@test.com");
            _mapper.Map<User>(command).Returns((User?)null);

            await _handler.Handle(command, CancellationToken.None);

            await _userService.Received(1).CreateUserAsync(null);
        }
    }
}
