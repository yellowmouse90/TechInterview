namespace TechInterview.GlobalSearch.Domain;

/// <summary>
/// Documento dell'indice di ricerca: un aeroporto o un volo, con i campi già normalizzati.
/// </summary>
/// <param name="Id">Codice IATA dell'aeroporto o codice volo (es. "MXP", "AZ178").</param>
/// <param name="ResourceType">Uno dei valori di <see cref="ResourceTypes"/>.</param>
/// <param name="Description">Testo leggibile restituito dall'API, costruito con i valori originali.</param>
/// <param name="Fields">Campi su cui si cerca, già normalizzati con <c>TextNormalizer</c>.</param>
/// <param name="DepartureTimeUtc">
/// Partenza del volo, usata per ordinare i voli a parità di rilevanza. Null per gli aeroporti
/// e per i voli con un orario illeggibile.
/// </param>
public sealed record SearchDocument(
    string Id,
    string ResourceType,
    string Description,
    IReadOnlyList<SearchField> Fields,
    DateTime? DepartureTimeUtc = null);
