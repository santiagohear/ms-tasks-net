using Application.Users.GetUsers;
using Application.Users.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Domain.Test.Handler.GetUsers
{
    public class GetUsersHandlerExceptionTests
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly GetUsersHandler _handler;

        public GetUsersHandlerExceptionTests()
        {
            _userService = Substitute.For<IUserService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetUsersHandler(_userService, _mapper);
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            var query = new GetUsersQuery();
            _userService.GetUsersAsync().ThrowsAsync(new InvalidOperationException("Service failure"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
            _mapper.DidNotReceive().Map<IEnumerable<UserDto>>(Arg.Any<IEnumerable<User>>());
        }

        [Fact]
        public async Task Handle_WhenMapperThrowsException_ShouldPropagateException()
        {
            var query = new GetUsersQuery();
            var users = new List<User>
            {
                new() { Id = 1, UserName = "Santi", Email = "santi@test.com" }
            };

            _userService.GetUsersAsync().Returns(users);
            _mapper.Map<IEnumerable<UserDto>>(users).Throws(new AutoMapperMappingException("Mapping failed"));

            await Assert.ThrowsAsync<AutoMapperMappingException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
