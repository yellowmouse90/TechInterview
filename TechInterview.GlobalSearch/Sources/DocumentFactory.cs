using System.Globalization;
using FlightsService.Grpc;
using TechInterview.GlobalSearch.Domain;
using TechInterview.GlobalSearch.Indexing;

namespace TechInterview.GlobalSearch.Sources;

/// <summary>
/// Traduce i contratti delle sorgenti (JSON degli aeroporti, messaggio gRPC dei voli) nel modello
/// di ricerca: campi normalizzati per il ranking e description leggibile con i valori originali.
/// </summary>
public static class DocumentFactory
{
    public static SearchDocument FromAirport(AirportDto airport) => new(
        airport.Id,
        ResourceTypes.Airport,
        $"{airport.Id} – {airport.Name} ({airport.City}, {airport.Country})",
        Fields(
            (FieldKind.PrimaryCode, airport.Id),
            (FieldKind.Name, airport.Name),
            (FieldKind.City, airport.City),
            (FieldKind.Country, airport.Country)));

    public static SearchDocument FromFlight(Flight flight)
    {
        var route = $"{flight.Id} – {flight.DepartureCity} ({flight.DepartureAirportCode}) → {flight.ArrivalCity} ({flight.ArrivalAirportCode})";

        // Un orario illeggibile toglie solo l'orario dalla description (e il volo va in fondo tra quelli con lo
        // stesso punteggio): non deve far fallire il refresh dei voli.
        DateTime? departureUtc = TryParseUtc(flight.DepartureTime, out var parsed) ? parsed : null;
        var description = departureUtc is { } time
            ? string.Create(CultureInfo.InvariantCulture, $"{route}, {time:yyyy-MM-dd HH:mm} UTC")
            : route;

        return new SearchDocument(
            flight.Id,
            ResourceTypes.Flight,
            description,
            Fields(
                (FieldKind.PrimaryCode, flight.Id),
                (FieldKind.PrimaryCode, flight.AircraftNumber),
                (FieldKind.City, flight.DepartureCity),
                (FieldKind.City, flight.ArrivalCity),
                (FieldKind.RelatedCode, flight.DepartureAirportCode),
                (FieldKind.RelatedCode, flight.ArrivalAirportCode)),
            departureUtc);
    }

    /// <summary>
    /// FlightsService serializza gli orari con il formato "o" (suffisso Z). DateTimeOffset evita
    /// DateTimeStyles.RoundtripKind | AdjustToUniversal, combinazione che lancia ArgumentException;
    /// AssumeUniversal legge come UTC un orario senza fuso, invece che come ora locale.
    /// </summary>
    private static bool TryParseUtc(string value, out DateTime utc)
    {
        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
        {
            utc = parsed.UtcDateTime;
            return true;
        }

        utc = default;
        return false;
    }

    private static SearchField[] Fields(params (FieldKind Kind, string Value)[] fields) =>
        fields
            .Select(f => new SearchField(f.Kind, TextNormalizer.Normalize(f.Value)))
            .Where(f => f.NormalizedValue.Length > 0)
            .ToArray();
}
