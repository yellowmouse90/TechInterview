namespace TechInterview.GlobalSearch.Domain;

/// <summary>
/// Ricerca già validata.
/// </summary>
/// <param name="NormalizedText">Testo normalizzato con <c>TextNormalizer</c>.</param>
/// <param name="Offset">Numero di risultati da saltare, applicato dopo l'ordinamento.</param>
/// <param name="Limit">Numero massimo di risultati da restituire.</param>
public sealed record SearchQuery(string NormalizedText, int Offset, int Limit);
