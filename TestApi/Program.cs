
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using System.Text;
using TestApi.MinimalApis;
using TestApi.Services;
using Infrastructure;
using Application.Services;
using Domain;
using Application;

namespace TestApi;

public class Program
{
    public static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddConfiguration(config);

        builder.Host.UseSerilog((_, logger) => logger.ReadFrom.Configuration(config));

        builder.Services.ConfigureInfrastructureServices(config)
            .ConfigureDomainServices()
            .ConfigureApplicationServices()
            .ConfigureTelemetry(config)
            .ConfigureAuth(config);

        builder.Services.AddControllers();
        builder.Services.AddRouting();

#if DEBUG
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
#endif

        // Build the application once all services are registered
        WebApplication app = builder.Build();

#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif
        app.UseSerilogRequestLogging();
        app.UseRouting();
                
        app.UseAuthorization();
        app.UseAuthentication();
        
        app.MapControllers();
        app.AddHealthChecks();
        app.AddGenericEndpoint();

        app.Run();
    }
}
