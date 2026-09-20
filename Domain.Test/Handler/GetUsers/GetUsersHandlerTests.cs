using Application.Users.GetUsers;
using Application.Users.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using NSubstitute;

namespace Domain.Test.Handler.GetUsers
{
    public class GetUsersHandlerTests
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly GetUsersHandler _handler;

        public GetUsersHandlerTests()
        {
            _userService = Substitute.For<IUserService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetUsersHandler(_userService, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidQuery_ShouldReturnMappedDtos()
        {
            var query = new GetUsersQuery();
            var users = new List<User>
            {
                new() { Id = 1, UserName = "Santi", Email = "santi@test.com" }
            };
            var dtos = new List<UserDto>
            {
                new() { Id = 1, UserName = "Santi", Email = "santi@test.com" }
            };

            _userService.GetUsersAsync().Returns(users);
            _mapper.Map<IEnumerable<UserDto>>(users).Returns(dtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            _mapper.Received(1).Map<IEnumerable<UserDto>>(users);
            await _userService.Received(1).GetUsersAsync();
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyCollection()
        {
            var query = new GetUsersQuery();
            var users = Array.Empty<User>();
            var dtos = Array.Empty<UserDto>();

            _userService.GetUsersAsync().Returns(users);
            _mapper.Map<IEnumerable<UserDto>>(users).Returns(dtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(result);
        }
    }
}
