using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DataSource;

namespace Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddSqlServer(services, configuration);

            services.AddAdapters();
            services.AddDomainServices();

            return services;
        }

        private static void AddSqlServer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PersistenceContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Base"),
                    sqlServerOptionsAction =>
                    {
                        sqlServerOptionsAction.MigrationsHistoryTable("__MicroMigrationHistory", configuration.GetConnectionString("BaseSchema"));
                    });

                options.ConfigureWarnings(warnings =>
                {
                    warnings.Default(WarningBehavior.Log);
                });
            });
        }
    }
}
