using System.Diagnostics;
using TechInterview.GlobalSearch.Indexing;
using TechInterview.GlobalSearch.Sources;

namespace TechInterview.GlobalSearch.Refresh;

/// <summary>
/// Mantiene aggiornato un dominio dell'indice (un'istanza per dominio): carica subito all'avvio, poi
/// ricarica dopo <see cref="RefreshSchedule.RefreshInterval"/>, o dopo <see cref="RefreshSchedule.RetryInterval"/>
/// se il caricamento è fallito. Un errore non tocca l'indice: le ricerche continuano sulla versione precedente.
/// </summary>
public sealed class IndexRefreshService : BackgroundService
{
    private readonly string _resourceType;
    private readonly RefreshSchedule _schedule;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PagedReader _reader;
    private readonly ISearchIndex _index;
    private readonly ILogger<IndexRefreshService> _logger;

    public IndexRefreshService(
        string resourceType,
        RefreshSchedule schedule,
        IServiceScopeFactory scopeFactory,
        PagedReader reader,
        ISearchIndex index,
        ILogger<IndexRefreshService> logger)
    {
        _resourceType = resourceType;
        _schedule = schedule;
        _scopeFactory = scopeFactory;
        _reader = reader;
        _index = index;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (true)
            {
                var succeeded = await RefreshOnceAsync(stoppingToken);

                // L'attesa dipende dall'esito, per questo Task.Delay e non PeriodicTimer.
                await Task.Delay(succeeded ? _schedule.RefreshInterval : _schedule.RetryInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Shutdown dell'applicazione: non è un errore.
        }
    }

    private async Task<bool> RefreshOnceAsync(CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // I client HTTP e gRPC sono transient: uno scope nuovo a ogni refresh, mai catturati in questo singleton.
            await using var scope = _scopeFactory.CreateAsyncScope();
            var source = scope.ServiceProvider.GetRequiredKeyedService<IDocumentSource>(_resourceType);

            var documents = await _reader.ReadAllAsync(source, ct);
            await _index.ReplaceAsync(_resourceType, documents, ct);

            _logger.LogInformation(
                "Search index refreshed: {Count} {ResourceType} documents in {ElapsedMs} ms",
                documents.Count, _resourceType, stopwatch.ElapsedMilliseconds);

            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException || !ct.IsCancellationRequested)
        {
            // Anche un timeout (TaskCanceledException senza shutdown) o una lettura scartata sono errori da ritentare.
            _logger.LogError(ex,
                "Refresh of {ResourceType} failed after {ElapsedMs} ms: previous index kept, retrying in {RetryInterval}",
                _resourceType, stopwatch.ElapsedMilliseconds, _schedule.RetryInterval);

            return false;
        }
    }
}
