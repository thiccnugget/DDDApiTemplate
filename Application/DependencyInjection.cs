using Application.Interfaces;
using Application.Options;
using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<UserAppService>();
            services.AddScoped<IAuthService, AuthService>();

            // Options pattern for JWTs
            services.AddOptions<JwtOptions>().BindConfiguration("Jwt")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
