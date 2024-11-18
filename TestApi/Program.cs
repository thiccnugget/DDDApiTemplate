
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

namespace TestApi;

public class Program
{
    public static void Main(string[] args)
    {
        // Load the configuration as needed
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddConfiguration(config);

        // Add OpenTelemetry metrics and tracing
        if (!string.IsNullOrEmpty(config.GetValue<string>("OpenTelemetry:Endpoint")))
        {
            Uri optlEndpoint = new Uri(config.GetValue<string>("OpenTelemetry:Endpoint"));

            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics => metrics
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = optlEndpoint)
                    )
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = optlEndpoint)
                    );
        }

        builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config["Jwt:Authority"];
                options.Audience = config["Jwt:Audience"];
                options.IncludeErrorDetails = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                };
            });

        builder.Services.AddControllers();

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
        app.MapControllers();

        app.Run();
    }
}
