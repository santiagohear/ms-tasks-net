using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DataSource
{
    public class PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration = configuration;

        public DbSet<User> Users => Set<User>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }
            modelBuilder.HasDefaultSchema(_configuration.GetConnectionString("BaseSchema"));
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
