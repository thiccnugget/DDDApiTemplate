using Application.Services;
using Domain.Entities;

namespace TestApi.MinimalApis
{
    public static class GenericEndpoint
    {
        public static void AddGenericEndpoint(this WebApplication app)
        {
            app.MapGet("/generic/{id}", async (Guid id, UserAppService service) =>
            {
                UserEntity? user = await service.RetrieveUserById(id);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            app.MapPost("/user", async (UserAppService service, string username, string email, string role, string password) =>
            {
                UserEntity? user = await service.CreateUser(username, password, email, role);
                return Results.Ok(user);
            });
        }
    }
}
