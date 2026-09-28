using FlightsService.Grpc;

namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Legge i voli da <c>Flights.GetFlights</c> di FlightsService (client gRPC, transient).
/// </summary>
public sealed class FlightsGrpcSource : IDocumentSource
{
    private readonly Flights.FlightsClient _client;

    public FlightsGrpcSource(Flights.FlightsClient client)
    {
        _client = client;
    }

    public async Task<SourcePage> GetPageAsync(int offset, int limit, CancellationToken ct)
    {
        var response = await _client.GetFlightsAsync(
            new GetFlightsRequest { Offset = offset, Limit = limit },
            cancellationToken: ct);

        return new SourcePage(response.Flights.Select(DocumentFactory.FromFlight).ToList(), response.TotalCount);
    }
}
