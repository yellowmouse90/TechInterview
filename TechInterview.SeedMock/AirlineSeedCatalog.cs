namespace TechInterview.SeedMock
{
    /// <summary>
    /// Catalogo condiviso delle compagnie aeree, indicizzato per nazione:
    /// una tratta viene operata dal vettore del paese dell'aeroporto di partenza,
    /// così codice volo e immatricolazione restano coerenti con la rotta.
    /// </summary>
    public static class AirlineSeedCatalog
    {
        private static readonly AirlineSeed[] Entries =
        {
            new("AZ", "ITA Airways", "Italy", "I-@@@@"),
            new("AF", "Air France", "France", "F-G@@@"),
            new("BA", "British Airways", "United Kingdom", "G-@@@@"),
            new("IB", "Iberia", "Spain", "EC-@@@"),
            new("LH", "Lufthansa", "Germany", "D-A@@@"),
            new("KL", "KLM", "Netherlands", "PH-@@@"),
            new("SN", "Brussels Airlines", "Belgium", "OO-@@@"),
            new("LX", "Swiss", "Switzerland", "HB-@@@"),
            new("OS", "Austrian Airlines", "Austria", "OE-L@@"),
            new("TP", "TAP Air Portugal", "Portugal", "CS-T@@"),
            new("EI", "Aer Lingus", "Ireland", "EI-@@@"),
            new("SK", "SAS", "Denmark", "OY-@@@"),
            new("SK", "SAS", "Sweden", "SE-@@@"),
            new("DY", "Norwegian", "Norway", "LN-@@@"),
            new("AY", "Finnair", "Finland", "OH-L@@"),
            new("LO", "LOT Polish Airlines", "Poland", "SP-L@@"),
            new("OK", "Czech Airlines", "Czech Republic", "OK-@@@"),
            new("W6", "Wizz Air", "Hungary", "HA-L@@"),
            new("A3", "Aegean Airlines", "Greece", "SX-@@@"),
            new("TK", "Turkish Airlines", "Turkey", "TC-@@@"),
            new("RO", "Tarom", "Romania", "YR-@@@"),
            new("UA", "United Airlines", "United States", "N###@@"),
            new("AC", "Air Canada", "Canada", "C-F@@@"),
            new("AM", "Aeromexico", "Mexico", "XA-@@@"),
            new("LA", "LATAM", "Brazil", "PR-@@@"),
            new("AR", "Aerolineas Argentinas", "Argentina", "LV-@@@"),
            new("EK", "Emirates", "United Arab Emirates", "A6-E@@"),
            new("QR", "Qatar Airways", "Qatar", "A7-B@@"),
            new("LY", "El Al", "Israel", "4X-E@@"),
            new("MS", "EgyptAir", "Egypt", "SU-G@@"),
            new("AT", "Royal Air Maroc", "Morocco", "CN-R@@"),
            new("SA", "South African Airways", "South Africa", "ZS-S@@"),
            new("KQ", "Kenya Airways", "Kenya", "5Y-K@@"),
            new("JL", "Japan Airlines", "Japan", "JA###@"),
            new("KE", "Korean Air", "South Korea", "HL####"),
            new("CA", "Air China", "China", "B-####"),
            new("CX", "Cathay Pacific", "Hong Kong", "B-L@@"),
            new("BR", "EVA Air", "Taiwan", "B-#####"),
            new("SQ", "Singapore Airlines", "Singapore", "9V-S@@"),
            new("TG", "Thai Airways", "Thailand", "HS-T@@"),
            new("MH", "Malaysia Airlines", "Malaysia", "9M-M@@"),
            new("GA", "Garuda Indonesia", "Indonesia", "PK-G@@"),
            new("AI", "Air India", "India", "VT-@@@"),
            new("QF", "Qantas", "Australia", "VH-@@@"),
            new("NZ", "Air New Zealand", "New Zealand", "ZK-N@@")
        };

        private static readonly Dictionary<string, AirlineSeed> ByCountry =
            Entries.ToDictionary(entry => entry.Country, StringComparer.Ordinal);

        /// <summary>Tutte le compagnie del catalogo.</summary>
        public static IReadOnlyList<AirlineSeed> All { get; } = Entries;

        /// <summary>
        /// Compagnia che opera le tratte in partenza dal paese indicato.
        /// Lancia se il paese non è nel catalogo: un aeroporto senza vettore
        /// è un'incoerenza dei dati, non un caso da gestire a runtime.
        /// </summary>
        public static AirlineSeed ForCountry(string country) => ByCountry[country];
    }
}
