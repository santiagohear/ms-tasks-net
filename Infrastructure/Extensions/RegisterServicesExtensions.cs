using Microsoft.Extensions.DependencyInjection;
using Domain.Attributes;

namespace Infrastructure.Extensions
{
    public static class RegisterServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection svc)
        {
            var _services = typeof(DomainServiceAttribute).Assembly
                .GetTypes()
                .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute))).ToList();

            _services.ForEach(service =>
            {
                var iFace = service.GetInterfaces()?.FirstOrDefault();

                if (iFace is null)
                {
                    svc.AddTransient(service);
                }
                else
                {
                    svc.AddTransient(iFace, service);
                }
            });

            return svc;
        }
    }
}
