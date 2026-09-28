using System.Text.Encodings.Web;
using FlightsService.Grpc;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using TechInterview.GlobalSearch.Api;
using TechInterview.GlobalSearch.Domain;
using TechInterview.GlobalSearch.Health;
using TechInterview.GlobalSearch.Indexing;
using TechInterview.GlobalSearch.Refresh;
using TechInterview.GlobalSearch.Sources;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddOptions<GlobalSearchOptions>()
    .BindConfiguration(GlobalSearchOptions.SectionName)
    .ValidateDataAnnotations()
    // ValidateDataAnnotations non entra negli oggetti annidati e non vede i vincoli tra campi.
    .Validate(options => options.Airports.IsValid && options.Flights.IsValid,
        "GlobalSearch refresh and retry intervals must be greater than zero.")
    .Validate(options => options.DefaultLimit <= options.MaxLimit,
        "GlobalSearch:DefaultLimit must not be greater than GlobalSearch:MaxLimit.")
    .ValidateOnStart();

// Sorgenti: i client HTTP e gRPC sono transient, quindi anche le sorgenti. IndexRefreshService le
// risolve per chiave di dominio in uno scope nuovo a ogni refresh: nessun client catturato in un singleton.
builder.Services.AddHttpClient<AirportsRestSource>(client =>
    client.BaseAddress = new Uri("https+http://airportsservice"));
builder.Services.AddKeyedTransient<IDocumentSource>(ResourceTypes.Airport,
    (sp, _) => sp.GetRequiredService<AirportsRestSource>());

// FlightsService accetta solo HTTP/2 in chiaro: stesso indirizzo usato da TechInterview.Web.
builder.Services.AddGrpcClient<Flights.FlightsClient>(options =>
    options.Address = new Uri("http://flightsservice"));
builder.Services.AddKeyedTransient<IDocumentSource, FlightsGrpcSource>(ResourceTypes.Flight);

builder.Services.AddSingleton<ISearchIndex, InMemorySearchIndex>();
builder.Services.AddSingleton<PagedReader>();

// Un refresh per dominio. AddHostedService<T>() registrerebbe lo stesso tipo una volta sola: si usa una factory.
builder.Services.AddSingleton<IHostedService>(sp => CreateRefreshService(sp, ResourceTypes.Airport, options => options.Airports));
builder.Services.AddSingleton<IHostedService>(sp => CreateRefreshService(sp, ResourceTypes.Flight, options => options.Flights));

builder.Services.AddHealthChecks()
    .AddCheck<SearchIndexHealthCheck>("search-index");

// La description contiene "–" e "→": senza questo encoder uscirebbero come – e →.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping);

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.UseHttpsRedirection();

app.MapGlobalSearch();

app.Run();

// Le opzioni si leggono dentro la factory, dal container: al momento della registrazione non esistono ancora.
static IndexRefreshService CreateRefreshService(
    IServiceProvider sp, string resourceType, Func<GlobalSearchOptions, RefreshSchedule> schedule) => new(
    resourceType,
    schedule(sp.GetRequiredService<IOptions<GlobalSearchOptions>>().Value),
    sp.GetRequiredService<IServiceScopeFactory>(),
    sp.GetRequiredService<PagedReader>(),
    sp.GetRequiredService<ISearchIndex>(),
    sp.GetRequiredService<ILogger<IndexRefreshService>>());
