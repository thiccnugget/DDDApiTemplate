var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(name: "redis");

var database = builder.AddPostgres(name: "database")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.TestApi>("testapi")
    .WithHttpHealthCheck("/health")
    .WithReference(source: cache, connectionName: "Redis")
    .WithReference(source: database, connectionName: "Database")
    .WaitFor(cache)
    .WaitFor(database);

builder.Build().Run();
