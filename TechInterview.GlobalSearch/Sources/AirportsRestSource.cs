namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Legge gli aeroporti da <c>GET /api/airports</c> di AirportsService (typed HttpClient, transient).
/// </summary>
public sealed class AirportsRestSource : IDocumentSource
{
    private readonly HttpClient _http;

    public AirportsRestSource(HttpClient http)
    {
        _http = http;
    }

    public async Task<SourcePage> GetPageAsync(int offset, int limit, CancellationToken ct)
    {
        var page = await _http.GetFromJsonAsync<AirportsPageDto>($"api/airports?offset={offset}&limit={limit}", ct)
                   ?? throw new InvalidOperationException("AirportsService returned an empty response.");

        return new SourcePage(page.Items.Select(DocumentFactory.FromAirport).ToList(), page.TotalCount);
    }
}
