var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TestApi>("testapi");

builder.Build().Run();
