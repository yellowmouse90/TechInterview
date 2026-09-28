namespace TechInterview.GlobalSearch.Domain;

/// <summary>
/// Campo di ricerca di un documento.
/// </summary>
/// <param name="Kind">Tipo di campo: determina il bonus di ranking.</param>
/// <param name="NormalizedValue">Valore già normalizzato, confrontato in modo ordinal con la query.</param>
public sealed record SearchField(FieldKind Kind, string NormalizedValue);

/// <summary>
/// Tipo di un campo di ricerca, in ordine di importanza per il ranking.
/// </summary>
public enum FieldKind
{
    /// <summary>Codice IATA dell'aeroporto, codice volo, immatricolazione dell'aeromobile.</summary>
    PrimaryCode,

    /// <summary>Nome dell'aeroporto.</summary>
    Name,

    /// <summary>Città dell'aeroporto o città di partenza/arrivo del volo.</summary>
    City,

    /// <summary>Codici degli aeroporti di partenza/arrivo di un volo.</summary>
    RelatedCode,

    /// <summary>Nazione dell'aeroporto.</summary>
    Country
}
