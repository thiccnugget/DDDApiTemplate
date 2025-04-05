using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace TestApi.Services
{
    internal static class ConfigureTelemetryExtension
    {
        public static IServiceCollection ConfigureTelemetry(this IServiceCollection services, IConfiguration config)
        {
            string? endpoint = config.GetValue<string>("OpenTelemetry:Endpoint");

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
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.Filter = httpContext =>
                            {
                                return !httpContext.Request.Path.StartsWithSegments("/health");
                            };
                        })
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddRedisInstrumentation()
                        .AddOtlpExporter(options => options.Endpoint = optlEndpoint)
                    );
            }

            return services;
        }
    }
}
