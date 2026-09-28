using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechInterview.GlobalSearch.Indexing;

namespace TechInterview.GlobalSearch.Health;

/// <summary>
/// Readiness del servizio: Unhealthy finché entrambi i domini non sono stati caricati almeno una volta.
/// Non ha il tag "live": /alive resta Healthy durante il caricamento.
/// </summary>
public sealed class SearchIndexHealthCheck : IHealthCheck
{
    private readonly ISearchIndex _index;

    public SearchIndexHealthCheck(ISearchIndex index)
    {
        _index = index;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) =>
        Task.FromResult(_index.IsReady
            ? HealthCheckResult.Healthy("Search index loaded.")
            : HealthCheckResult.Unhealthy("Search index is loading."));
}
