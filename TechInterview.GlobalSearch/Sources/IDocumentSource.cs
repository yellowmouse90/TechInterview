using TechInterview.GlobalSearch.Domain;

namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Sorgente paginata di documenti: ogni implementazione traduce il proprio contratto (REST, gRPC)
/// in <see cref="SearchDocument"/>, così a valle non c'è differenza tra le sorgenti.
/// </summary>
public interface IDocumentSource
{
    Task<SourcePage> GetPageAsync(int offset, int limit, CancellationToken ct);
}

/// <summary>
/// Pagina letta da una sorgente.
/// </summary>
/// <param name="Items">Documenti della pagina.</param>
/// <param name="TotalCount">Numero totale di elementi della sorgente al momento della chiamata.</param>
public sealed record SourcePage(IReadOnlyList<SearchDocument> Items, int TotalCount);
