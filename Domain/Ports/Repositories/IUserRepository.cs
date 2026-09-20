using Domain.Entities;

namespace Domain.Ports.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> FindByEmailAsync(string email);
    }
}
