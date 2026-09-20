using Domain.Entities;
using Domain.Ports.Repositories;
using Domain.Services;
using NSubstitute;

namespace Domain.Test.Services
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IRepository<User> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _repositoryMock = Substitute.For<IRepository<User>>();
            _userService = new UserService(_userRepositoryMock, _repositoryMock);
        }

        [Fact]
        public async Task CreateUserAsync_WithUniqueEmail_ShouldAddUser()
        {
            var user = new User
            {
                Id = 1,
                UserName = "Santi",
                Email = "santi@test.com"
            };

            _userRepositoryMock.FindByEmailAsync(user.Email).Returns((User?)null);

            await _userService.CreateUserAsync(user);

            await _repositoryMock.Received(1).AddAsync(user);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsersFromRepository()
        {
            var userA = new User { Id = 1, UserName = "A", Email = "a@test.com" };
            var userB = new User { Id = 2, UserName = "B", Email = "b@test.com" };
            var users = new[] { userA, userB };

            _userRepositoryMock.GetUsersAsync().Returns(users);

            var result = await _userService.GetUsersAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FindUserAsync_WithExistingUser_ShouldReturnUser()
        {
            var expected = new User { Id = 7, UserName = "User 7", Email = "user7@test.com" };
            _repositoryMock.FindAsync(7).Returns(expected);

            var result = await _userService.FindUserAsync(7);

            Assert.Equal(expected, result);
        }
    }
}
