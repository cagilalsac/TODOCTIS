var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_TODO>("api-todo");

builder.AddProject<Projects.API_Users>("api-users");

builder.Build().Run();
