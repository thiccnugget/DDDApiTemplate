using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<UserDomainService>();

            return services;
        }
    }
}
