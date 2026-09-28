namespace TechInterview.GlobalSearch.Api;

/// <summary>
/// Risposta di <c>GET /api/global-search</c>.
/// </summary>
/// <param name="Items">Risultati della pagina, ordinati per rilevanza.</param>
/// <param name="Offset">Offset applicato.</param>
/// <param name="Limit">Limit applicato, già troncato a GlobalSearch:MaxLimit.</param>
/// <param name="Count">Numero di elementi della pagina (lettura letterale dell'esempio del README).</param>
/// <param name="TotalCount">Numero totale dei match della query, lo stesso nome usato dai servizi esistenti.</param>
public sealed record GlobalSearchResponse(
    IReadOnlyList<GlobalSearchItem> Items,
    int Offset,
    int Limit,
    int Count,
    int TotalCount);

/// <summary>
/// Singolo risultato, aeroporto o volo.
/// </summary>
/// <param name="Id">Codice IATA dell'aeroporto o codice volo.</param>
/// <param name="ResourceType">"airport" o "flight".</param>
/// <param name="Description">Testo leggibile, es. "MXP – Milano Malpensa (Milano, Italy)".</param>
public sealed record GlobalSearchItem(string Id, string ResourceType, string Description);
