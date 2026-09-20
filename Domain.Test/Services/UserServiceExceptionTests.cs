using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports.Repositories;
using Domain.Services;
using NSubstitute;

namespace Domain.Test.Services
{
    public class UserServiceExceptionTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IRepository<User> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceExceptionTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _repositoryMock = Substitute.For<IRepository<User>>();
            _userService = new UserService(_userRepositoryMock, _repositoryMock);
        }

        [Fact]
        public async Task CreateUserAsync_WithNullUser_ShouldThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateUserAsync(null));
        }

        [Fact]
        public async Task CreateUserAsync_WithEmptyUserName_ShouldThrowValidationException()
        {
            var user = new User { Email = "user@test.com", UserName = string.Empty };

            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateUserAsync(user));
        }

        [Fact]
        public async Task CreateUserAsync_WithEmptyEmail_ShouldThrowValidationException()
        {
            var user = new User { UserName = "User", Email = string.Empty };

            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateUserAsync(user));
        }

        [Fact]
        public async Task CreateUserAsync_WhenEmailAlreadyExists_ShouldThrowValidationException()
        {
            var user = new User { Id = 2, UserName = "New", Email = "existing@test.com" };
            _userRepositoryMock.FindByEmailAsync(user.Email).Returns(new User { Id = 1, UserName = "Existing", Email = user.Email });

            await Assert.ThrowsAsync<ValidationException>(() => _userService.CreateUserAsync(user));
        }

        [Fact]
        public async Task FindUserAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
        {
            _repositoryMock.FindAsync(999).Returns((User?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _userService.FindUserAsync(999));
        }
    }
}
