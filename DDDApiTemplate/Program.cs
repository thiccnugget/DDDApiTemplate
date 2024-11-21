using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add Redis with more specific configuration
var cache = builder.AddRedis("redis")
    .WithEnvironment("REDIS_PASSWORD", "SuperInsecure!")
    .WithEndpoint("redis", x => x.TargetPort = 6379 );

// Add SQL Server with more robust configuration
var database = builder.AddSqlServer("database", port: 1433)
    .WithDataVolume()
    .AddDatabase("testDb");

// Add your API project with health checks
builder.AddProject<Projects.TestApi>("testapi")
    .WithReference(cache)
    .WithReference(database)
    .WaitFor(cache)
    .WaitFor(database);

builder.Build().Run();
