using Infrastructure.Cache;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class InfrastructureServicesExtension
    {
        public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("Database")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")));
            services.AddSingleton<ICacheService, CacheService>();

        }
    }
}
