using Microsoft.AspNetCore.Authorization;

namespace TestApi.MinimalApis
{
    internal static class ApiHealthChecks
    {
        public static void AddHealthChecks(this WebApplication app)
        { 
            var group = app.MapGroup("/health");

            group.MapGet("/", [AllowAnonymous] () => Results.Ok("OK")).ExcludeFromDescription();
        }
    }
}
