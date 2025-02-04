var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.tools_ApiService>("api");

builder.AddProject<Projects.tools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);
//Storage required a package - packages\aspire.hosting.azure.storage\9.0.0\
var storage = builder.AddAzureStorage("storage")
    .AddBlobs("content-store");


builder.Build().Run();
