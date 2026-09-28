using Microsoft.Extensions.Options;
using TechInterview.GlobalSearch.Domain;
using TechInterview.GlobalSearch.Refresh;

namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Legge tutti i documenti di una sorgente paginata. I servizi aggiungono un delay di 100 ms + 50 ms
/// per elemento richiesto, quindi solo il parallelismo riduce il tempo: una sonda con limit 1 legge il
/// totale (~150 ms), poi tutte le pagine partono insieme (con i default ~5 s invece di ~50 s in sequenza).
/// </summary>
public sealed class PagedReader
{
    private readonly IOptions<GlobalSearchOptions> _options;

    public PagedReader(IOptions<GlobalSearchOptions> options)
    {
        _options = options;
    }

    public async Task<IReadOnlyList<SearchDocument>> ReadAllAsync(IDocumentSource source, CancellationToken ct)
    {
        var options = _options.Value;

        var probe = await source.GetPageAsync(0, 1, ct);
        var totalCount = probe.TotalCount;
        if (totalCount == 0)
            return [];

        // L'ultima pagina chiede solo gli elementi rimanenti: il delay dipende dal limit richiesto, non da quelli restituiti.
        var requests = Enumerable
            .Range(0, (totalCount + options.PageSize - 1) / options.PageSize)
            .Select(page => page * options.PageSize)
            .Select(offset => (Offset: offset, Limit: Math.Min(options.PageSize, totalCount - offset)))
            .ToArray();

        var pages = new SourcePage[requests.Length];

        await Parallel.ForEachAsync(
            Enumerable.Range(0, requests.Length),
            new ParallelOptions { MaxDegreeOfParallelism = options.MaxParallelism, CancellationToken = ct },
            async (i, token) => pages[i] = await source.GetPageAsync(requests[i].Offset, requests[i].Limit, token));

        var documents = pages.SelectMany(page => page.Items).ToList();

        // Se la sorgente cambia durante la lettura, gli elementi si spostano tra le pagine (duplicati e mancanze):
        // la lettura si scarta, l'indice precedente resta e il refresh riprova.
        var uniqueIds = documents.DistinctBy(document => document.Id).Count();
        if (uniqueIds != totalCount || documents.Count != totalCount)
        {
            throw new InvalidOperationException(
                $"Read {documents.Count} items with {uniqueIds} unique ids, expected {totalCount}: the source changed while paging.");
        }

        return documents;
    }
}
