namespace TechInterview.GlobalSearch.Domain;

/// <summary>
/// Pagina di risultati di una ricerca.
/// </summary>
/// <param name="Items">Documenti della pagina, già ordinati per rilevanza.</param>
/// <param name="TotalCount">Numero totale dei match della query, indipendente da offset e limit.</param>
public sealed record SearchResult(IReadOnlyList<SearchDocument> Items, int TotalCount);
