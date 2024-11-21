namespace TestApi.MinimalApis
{
    internal static class ApiHealthChecks
    {
        public static void AddHealthChecks(this WebApplication app)
        {
            var group = app.MapGroup("/health");
            
            group.MapGet("/", () => Results.Ok("OK")).ExcludeFromDescription();
            group.MapGet("/ping", () => Results.Ok("Pong")).ExcludeFromDescription();
        }
    }
}
