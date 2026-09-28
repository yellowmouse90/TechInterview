using TechInterview.GlobalSearch.Domain;

namespace TechInterview.GlobalSearch.Indexing;

/// <summary>
/// Indice in memoria: un array immutabile di documenti per dominio. Un refresh pubblica un array
/// nuovo con uno swap atomico, quindi le ricerche non usano lock e non vedono mai uno stato a metà.
/// Con ~1.100 documenti la scansione lineare costa microsecondi: un indice invertito non serve.
/// </summary>
public sealed class InMemorySearchIndex : ISearchIndex
{
    // null finché il dominio non è stato caricato la prima volta.
    private SearchDocument[]? _airports;
    private SearchDocument[]? _flights;

    public bool IsReady => Volatile.Read(ref _airports) is not null && Volatile.Read(ref _flights) is not null;

    public Task ReplaceAsync(string resourceType, IReadOnlyCollection<SearchDocument> documents, CancellationToken ct)
    {
        if (documents.Any(d => d.ResourceType != resourceType))
            throw new ArgumentException($"All documents must have resource type '{resourceType}'.", nameof(documents));

        var snapshot = documents.ToArray();

        switch (resourceType)
        {
            case ResourceTypes.Airport:
                Volatile.Write(ref _airports, snapshot);
                break;
            case ResourceTypes.Flight:
                Volatile.Write(ref _flights, snapshot);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, "Unknown resource type.");
        }

        return Task.CompletedTask;
    }

    public Task<SearchResult> SearchAsync(SearchQuery query, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        // Ogni riferimento si legge una sola volta: un refresh concorrente non cambia i dati di questa ricerca.
        var airports = Volatile.Read(ref _airports) ?? [];
        var flights = Volatile.Read(ref _flights) ?? [];

        // Il ranking va calcolato su tutti i match prima della paginazione, altrimenti le pagine non sarebbero stabili.
        // A parità di punteggio: aeroporti prima dei voli, voli per partenza (i prossimi prima, quelli senza orario in
        // fondo), infine l'Id per un ordine sempre deterministico.
        var matches = airports
            .Concat(flights)
            .Select(document => (Document: document, Score: SearchRanker.Score(document, query.NormalizedText)))
            .Where(match => match.Score > 0)
            .OrderByDescending(match => match.Score)
            .ThenBy(match => match.Document.ResourceType == ResourceTypes.Airport ? 0 : 1)
            .ThenBy(match => match.Document.DepartureTimeUtc ?? DateTime.MaxValue)
            .ThenBy(match => match.Document.Id, StringComparer.Ordinal)
            .Select(match => match.Document)
            .ToList();

        var page = matches
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToList();

        return Task.FromResult(new SearchResult(page, matches.Count));
    }
}
