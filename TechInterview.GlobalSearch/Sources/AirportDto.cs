namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Aeroporto come lo restituisce <c>GET /api/airports</c> di AirportsService.
/// Copia locale del contratto JSON: nessun riferimento al progetto del servizio.
/// </summary>
public sealed record AirportDto(string Id, string Name, string City, string Country);

/// <summary>
/// Pagina restituita da <c>GET /api/airports?offset=&amp;limit=</c>, con i soli campi usati.
/// </summary>
public sealed record AirportsPageDto(IReadOnlyList<AirportDto> Items, int TotalCount);
