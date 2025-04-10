﻿using Infrastructure.Cache;
using Infrastructure.Persistence;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Domain.Interfaces;
using Infrastructure.Services;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(config.GetConnectionString("Database")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")));
            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddTransient<ICacheKeyGenerator, CacheKeyGenerator>();

            services.AddSingleton<IPasswordService, BcryptPasswordService>();

            // return the services collection so we can chain this method in the Program.cs file
            return services;
        }
    }
}
