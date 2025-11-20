var builder = DistributedApplication.CreateBuilder(args);

var airports = builder.AddProject<Projects.AirportsService>("airportsservice");

var flights = builder.AddProject<Projects.FlightsService>("flightsservice");

var apiService = builder.AddProject<Projects.TechInterview_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.TechInterview_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(flights)
    .WaitFor(flights)
    .WithReference(airports)
    .WaitFor(airports);

builder.Build().Run();