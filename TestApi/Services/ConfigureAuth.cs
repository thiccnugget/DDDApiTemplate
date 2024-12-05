using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
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

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("AdminOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .RequireRole("Admin"));

                options.AddPolicy("UserOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .RequireRole("User"));

            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Key not found"))),
                    
                };
            });

            return services;
        }
    }
}
