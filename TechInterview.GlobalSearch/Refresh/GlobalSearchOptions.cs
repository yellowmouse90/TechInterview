using System.ComponentModel.DataAnnotations;

namespace TechInterview.GlobalSearch.Refresh;

/// <summary>
/// Sezione "GlobalSearch" di appsettings.json. I vincoli tra campi e quelli dei
/// <see cref="RefreshSchedule"/> annidati sono validati in Program.cs: ValidateDataAnnotations non li copre.
/// </summary>
public sealed class GlobalSearchOptions
{
    public const string SectionName = "GlobalSearch";

    public RefreshSchedule Airports { get; set; } = new()
    {
        RefreshInterval = TimeSpan.FromHours(12),
        RetryInterval = TimeSpan.FromSeconds(30)
    };

    public RefreshSchedule Flights { get; set; } = new()
    {
        RefreshInterval = TimeSpan.FromMinutes(5),
        RetryInterval = TimeSpan.FromSeconds(30)
    };

    /// <summary>Elementi per pagina chiesti alle sorgenti: al massimo 100, il tetto applicato dai servizi.</summary>
    [Range(1, 100)]
    public int PageSize { get; set; } = 100;

    /// <summary>Pagine scaricate contemporaneamente da una sorgente.</summary>
    [Range(1, int.MaxValue)]
    public int MaxParallelism { get; set; } = 10;

    /// <summary>Limit della ricerca quando la richiesta non lo specifica.</summary>
    [Range(1, int.MaxValue)]
    public int DefaultLimit { get; set; } = 10;

    /// <summary>Limit massimo della ricerca: un valore più alto viene troncato.</summary>
    [Range(1, int.MaxValue)]
    public int MaxLimit { get; set; } = 100;

    /// <summary>Lunghezza minima della query normalizzata.</summary>
    [Range(1, int.MaxValue)]
    public int MinQueryLength { get; set; } = 3;
}

/// <summary>
/// Pianificazione del refresh di un dominio.
/// </summary>
public sealed class RefreshSchedule
{
    /// <summary>Attesa dopo un refresh riuscito.</summary>
    public TimeSpan RefreshInterval { get; set; }

    /// <summary>Attesa dopo un refresh fallito.</summary>
    public TimeSpan RetryInterval { get; set; }

    public bool IsValid => RefreshInterval > TimeSpan.Zero && RetryInterval > TimeSpan.Zero;
}
