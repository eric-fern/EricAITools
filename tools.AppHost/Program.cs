var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.tools_ApiService>("apiservice");

builder.AddProject<Projects.tools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
