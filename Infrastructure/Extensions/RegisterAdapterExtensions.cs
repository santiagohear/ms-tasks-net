using Microsoft.Extensions.DependencyInjection;
using Domain.Ports.Repositories;
using Infrastructure.Adapters;
using Infrastructure.Adapters.Repositories;
using Infrastructure.Attributes;

namespace Infrastructure.Extensions
{
    public static class RegisterAdapterExtensions
    {
        public static IServiceCollection AddAdapters(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var _repositories = typeof(AdapterAttribute).Assembly
                .GetTypes()
                .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(AdapterAttribute)))
                .ToList();

            _repositories.ForEach(repository =>
            {
                var iFace = repository.GetInterfaces()?
                    .Where(i => !i.IsGenericType || i.GetGenericTypeDefinition() != typeof(IRepository<>))
                    .FirstOrDefault(i => i != typeof(IDisposable));

                if (iFace is null)
                {
                    services.AddTransient(repository);
                }
                else
                {
                    services.AddTransient(iFace, repository);
                }
            });

            return services;
        }
    }
}
