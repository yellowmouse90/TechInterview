using TechInterview.GlobalSearch.Domain;

namespace TechInterview.GlobalSearch.Indexing;

/// <summary>
/// Indice di ricerca globale. Asincrono anche se l'implementazione attuale è in memoria:
/// l'architettura evolutiva (docs/evolution-architecture.md) lo implementa su PostgreSQL
/// senza cambiare i chiamanti.
/// </summary>
public interface ISearchIndex
{
    /// <summary>True quando entrambi i domini sono stati caricati almeno una volta.</summary>
    bool IsReady { get; }

    /// <summary>
    /// Sostituisce tutti i documenti del dominio con quelli ricevuti. Riceve solo scansioni
    /// complete e coerenti: il PagedReader scarta le altre prima che arrivino qui.
    /// </summary>
    Task ReplaceAsync(string resourceType, IReadOnlyCollection<SearchDocument> documents, CancellationToken ct);

    /// <summary>
    /// Ordina per rilevanza tutti i match della query, poi applica offset e limit.
    /// </summary>
    Task<SearchResult> SearchAsync(SearchQuery query, CancellationToken ct);
}
