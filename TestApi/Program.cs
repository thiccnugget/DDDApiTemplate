using Serilog;
using TestApi.MinimalApis;
using TestApi.Services;
using Infrastructure;
using Domain;
using Application;
using TestApi.Middlewares;

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

        // Configure services of each layer of the application
        builder.Services
            .ConfigureInfrastructureServices(config)
            .ConfigureDomainServices()
            .ConfigureApplicationServices()
            .ConfigureTelemetry(config)
            .ConfigureAuth(config);

        builder.Services.AddControllers();
        builder.Services.AddRouting();

#if DEBUG
        builder.Services.ConfigureSwaggerUI();
#endif

        // Build the application once all services are registered
        WebApplication app = builder.Build();

#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif

        app.UseSerilogRequestLogging();
        app.UseRouting();

        // Add middlewares BETWEEN routing and controllers/endpoints
        app.UseMiddleware<RequestTimingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        app.AddHealthChecks();
        app.AddGenericEndpoint();

        app.Run();
    }
}
