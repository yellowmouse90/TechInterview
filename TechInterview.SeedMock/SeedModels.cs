namespace TechInterview.SeedMock
{
    /// <summary>
    /// Aeroporto del catalogo condiviso. Il <paramref name="Code"/> (IATA) è la chiave
    /// con cui i voli referenziano gli aeroporti: è l'unico punto in cui è definita.
    /// </summary>
    /// <param name="Code">Codice IATA, es. "MXP".</param>
    /// <param name="Name">Nome dell'aeroporto, es. "Milano Malpensa".</param>
    /// <param name="City">Città servita.</param>
    /// <param name="Country">Nazione.</param>
    /// <param name="Latitude">Latitudine in gradi decimali.</param>
    /// <param name="Longitude">Longitudine in gradi decimali.</param>
    /// <param name="IsHub">True se l'aeroporto opera collegamenti intercontinentali.</param>
    public sealed record AirportSeed(
        string Code,
        string Name,
        string City,
        string Country,
        double Latitude,
        double Longitude,
        bool IsHub);

    /// <summary>
    /// Compagnia aerea del catalogo condiviso.
    /// </summary>
    /// <param name="Code">Codice IATA della compagnia, usato come prefisso del codice volo (es. "AZ" -> "AZ178").</param>
    /// <param name="Name">Ragione sociale.</param>
    /// <param name="Country">Nazione di immatricolazione della flotta.</param>
    /// <param name="RegistrationTemplate">
    /// Template dell'immatricolazione aeromobile: '#' = cifra, '@' = lettera, gli altri
    /// caratteri sono letterali. Es. "I-@@@@" -> "I-BIKA".
    /// </param>
    public sealed record AirlineSeed(
        string Code,
        string Name,
        string Country,
        string RegistrationTemplate);
}
