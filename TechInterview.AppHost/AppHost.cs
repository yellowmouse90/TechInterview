using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var airports = builder.AddProject<Projects.AirportsService>("airportsservice").WithUrlForEndpoint("scalar", url =>
{
    url.Url = "/scalar"; // Appends to the existing host and port
    url.DisplayText = "Scalar UI (HTTPS)";
});

var flights = builder.AddProject<Projects.FlightsService>("flightsservice");

builder.AddProject<Projects.TechInterview_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(flights)
    .WaitFor(flights)
    .WithReference(airports)
    .WaitFor(airports);

builder.Build().Run();