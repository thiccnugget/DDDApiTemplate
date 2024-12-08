using Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestApi.Services
{
    internal static class ConfigureAuthExtension
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration config)
        {
            // Add authorization policies as needed
            services.AddAuthorization(options =>
            {
                // Default policy is applied when no policy is specified in the [Authorize] attribute
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

            // Configure JWT authentication
            JwtOptions jwtOptions = config.GetSection("Jwt").Get<JwtOptions>();

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
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };
            });

            return services;
        }
    }
}
