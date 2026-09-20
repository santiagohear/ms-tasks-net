using Domain.Attributes;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports.Repositories;
using Domain.Ports.Services;

namespace Domain.Services
{
    [DomainService]
    public class UserService(IUserRepository userRepository, IRepository<User> repository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IRepository<User> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task CreateUserAsync(User? user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new ValidationException("El nombre del usuario es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ValidationException("El correo electrónico del usuario es obligatorio");
            }

            var existingUser = await _userRepository.FindByEmailAsync(user.Email);
            if (existingUser is not null)
            {
                throw new ValidationException($"Ya existe un usuario con el correo {user.Email}");
            }

            await _repository.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> FindUserAsync(int id)
        {
            return await _repository.FindAsync(id) ?? throw new NotFoundException($"No se encontró un usuario con Id: {id}");
        }
    }
}
