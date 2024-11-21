using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestApi.Services
{
    internal static class ConfigureAuthorizationExtension
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration config)
        {
            //services.AddAuthentication(cfg => {
            //    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options => {
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = false;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Secret"))
            //        ),
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ClockSkew = TimeSpan.Zero,
            //    };
            //});
        }
    }
}
