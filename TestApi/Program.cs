
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using StackExchange.Redis;
using System.Text;
using TestApi.MinimalApis;
using TestApi.Services;

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

        builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));

        builder.Services.ConfigureInfrastructureServices(config);
        builder.Services.ConfigureTelemetry(config);
        builder.Services.ConfigureAuthorization(config);

        builder.Services.AddControllers();
        builder.Services.AddRouting();

#if DEBUG
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
#endif

        WebApplication app = builder.Build();

#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif

        app.UseSerilogRequestLogging();
        app.UseAuthorization();

        app.AddHealthChecks();

        app.MapControllers();
        app.UseRouting();

        app.Run();
    }
}
