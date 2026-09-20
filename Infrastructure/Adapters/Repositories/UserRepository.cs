using Domain.Entities;
using Domain.Ports.Repositories;
using Infrastructure.Attributes;
using Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Repositories
{
    [Adapter]
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PersistenceContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await DbContext.Set<User>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await DbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
