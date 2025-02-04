var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.tools_ApiService>("api");

builder.AddProject<Projects.tools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

var storage = builder.AddAzureStorage("storage")
    .AddBlobs("content-store");


builder.Build().Run();
