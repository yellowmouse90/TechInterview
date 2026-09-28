using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var airports = builder.AddProject<Projects.AirportsService>("airportsservice").WithUrlForEndpoint("scalar", url =>
{
    url.Url = "/scalar"; // Appends to the existing host and port
    url.DisplayText = "Scalar UI (HTTPS)";
});

var flights = builder.AddProject<Projects.FlightsService>("flightsservice");

// Healthy solo dopo il primo caricamento completo dell'indice (circa 5 secondi).
var globalSearch = builder.AddProject<Projects.TechInterview_GlobalSearch>("globalsearch")
    .WithReference(airports)
    .WaitFor(airports)
    .WithReference(flights)
    .WaitFor(flights)
    .WithHttpHealthCheck("/health")
    .WithUrlForEndpoint("https", url =>
    {
        url.Url = "/scalar";
        url.DisplayText = "Scalar UI (HTTPS)";
    });

builder.AddProject<Projects.TechInterview_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(flights)
    .WaitFor(flights)
    .WithReference(airports)
    .WaitFor(airports)
    // Senza WaitFor: un problema della ricerca non deve bloccare le pagine Airports e Flights.
    .WithReference(globalSearch);

builder.Build().Run();