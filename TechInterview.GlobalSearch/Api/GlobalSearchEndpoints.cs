using System.ComponentModel;
using Microsoft.Extensions.Options;
using TechInterview.GlobalSearch.Domain;
using TechInterview.GlobalSearch.Indexing;
using TechInterview.GlobalSearch.Refresh;

namespace TechInterview.GlobalSearch.Api;

public static class GlobalSearchEndpoints
{
    public static IEndpointRouteBuilder MapGlobalSearch(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/global-search", SearchAsync)
            .WithName("GlobalSearch")
            .WithSummary("Searches airports and flights")
            .WithDescription(
                "Returns airports (code, name, city, country) and flights (flight code, aircraft number, " +
                "departure/arrival city and airport code) matching the query, ordered by relevance. " +
                "count is the number of items in the page, totalCount the total number of matches.")
            .Produces<GlobalSearchResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable);

        return app;
    }

    private static async Task<IResult> SearchAsync(
        [Description("Text to search, at least 3 characters (leading and trailing spaces excluded).")] string? query,
        [Description("Number of results to skip. Default 0.")] int? offset,
        [Description("Maximum number of results. Default 10, values above 100 are truncated to 100.")] int? limit,
        ISearchIndex index,
        IOptions<GlobalSearchOptions> options,
        CancellationToken ct)
    {
        var settings = options.Value;

        // La lunghezza minima si misura sulla query normalizzata: è il testo che si cerca davvero.
        var normalizedQuery = TextNormalizer.Normalize(query);

        var errors = new Dictionary<string, string[]>();
        if (normalizedQuery.Length < settings.MinQueryLength)
            errors["query"] = [$"The query must contain at least {settings.MinQueryLength} characters, leading and trailing spaces excluded."];
        if (offset < 0)
            errors["offset"] = ["The offset must be zero or greater."];
        if (limit < 1)
            errors["limit"] = ["The limit must be at least 1."];

        if (errors.Count > 0)
            return Results.ValidationProblem(errors);

        if (!index.IsReady)
        {
            return Results.Problem(
                title: "Search index is loading",
                detail: "The search index is being loaded from the Airports and Flights services. Retry shortly.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        // Come i servizi esistenti: un limit oltre il massimo viene troncato, non rifiutato.
        var effectiveOffset = offset ?? 0;
        var effectiveLimit = Math.Min(limit ?? settings.DefaultLimit, settings.MaxLimit);

        var result = await index.SearchAsync(new SearchQuery(normalizedQuery, effectiveOffset, effectiveLimit), ct);

        var items = result.Items
            .Select(document => new GlobalSearchItem(document.Id, document.ResourceType, document.Description))
            .ToList();

        return Results.Ok(new GlobalSearchResponse(items, effectiveOffset, effectiveLimit, items.Count, result.TotalCount));
    }
}
