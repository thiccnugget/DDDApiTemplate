using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace TestApi.Services
{
    internal static class ConfigureTelemetryExtension
    {
        public static void ConfigureTelemetry(this IServiceCollection services, IConfiguration config)
        {
            string? endpoint = config.GetValue<string>("AzureAppConfiguration:Endpoint");

            if (!string.IsNullOrEmpty(endpoint))
            {
                Uri optlEndpoint = new Uri(endpoint);

                services.AddOpenTelemetry()
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
        }
    }
}
