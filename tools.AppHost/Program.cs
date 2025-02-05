//builder will make distributed app
var builder = DistributedApplication.CreateBuilder(args);

//Storage required a package - packages\aspire.hosting.azure.storage\9.0.0\
//add storage and a folder of blobs.
var storage = builder.AddAzureStorage("storage")
    .AddBlobs("content-store");
// Your existing API service definition gets updated to include the storage reference
var apiService = builder.AddProject<Projects.tools_ApiService>("api")
    .WithReference(storage);  // This line connects the API to the storage

builder.AddProject<Projects.tools_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);



builder.Build().Run();
