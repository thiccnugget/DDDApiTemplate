using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestApi.Services
{
    internal static class ConfigureAuthExtension
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration config)
        {
            // Configure JWT authentication
            var jwtSettings = config.GetSection("JwtSettings");
            string? secretKey = jwtSettings.GetValue<string>("SecretKey");

            if(string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret key is missing");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                    ValidAudience = jwtSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    AuthenticationType = "Bearer"
                };
            });

            // Add authorization policies as needed
            services.AddAuthorization(x => x.AddPolicy("AdminPolicy", p => p.RequireRole("Admin")));

            return services;
        }
    }
}
