using Domain.Entities;

namespace Domain.Ports.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(User? user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> FindUserAsync(int id);
    }
}
