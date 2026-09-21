using FlightsService.Grpc;
using Grpc.Core;
using TechInterview.SeedMock;

namespace FlightsService.Services
{
    public class FlightsServiceImpl : Flights.FlightsBase
    {
        private const int FlightsCount = 1000;
        private const int ScheduleWindowDays = 30;
        private const double CruiseSpeedKmh = 800;
        private const int GroundTimeMinutes = 25;

        /// <summary>Sotto questa distanza si viaggia in treno: nessun collegamento aereo di linea.</summary>
        private const double MinRouteDistanceKm = 300;

        /// <summary>Oltre questa distanza la tratta è considerata long haul (solo tra hub).</summary>
        private const double LongHaulThresholdKm = 4500;

        /// <summary>Raggio massimo operabile senza scalo: oltre, la tratta non è realistica.</summary>
        private const double MaxNonstopRangeKm = 15000;

        /// <summary>Seed fisso: lo stesso dataset a ogni avvio, così i risultati sono ripetibili.</summary>
        private static readonly Random Rand = new(20250921);

        /// <summary>
        /// Il dataset è statico perché le implementazioni gRPC vengono istanziate
        /// a ogni chiamata: con un campo di istanza ogni richiesta avrebbe visto
        /// voli diversi (paginazione e GetFlightById incoerenti tra loro).
        /// </summary>
        private static readonly List<FlightsService.Models.Flight> _flights = GenerateMockFlights(FlightsCount);

        public override async Task<GetFlightsResponse> GetFlights(GetFlightsRequest request, ServerCallContext context)
        {
            int offset = Math.Max(request.Offset, 0);
            int limit = request.Limit <= 0 ? 50 : request.Limit;
            limit = Math.Min(limit, 100);

            
            await Task.Delay(TimeSpan.FromMilliseconds(100 + 50 * limit));
            
            var paged = _flights
                .Skip(offset)
                .Take(limit)
                .ToList();

            var response = new GetFlightsResponse
            {
                Offset = offset,
                Limit = limit,
                TotalCount = _flights.Count
            };

            response.Flights.AddRange(paged.Select(f => new Flight
            {
                Id = f.Id,
                AircraftNumber = f.AircraftNumber,
                DepartureAirportCode = f.DepartureAirportCode,
                ArrivalAirportCode = f.ArrivalAirportCode,
                DepartureCity = f.DepartureCity,
                ArrivalCity = f.ArrivalCity,
                DepartureTime = f.DepartureTime.ToString("o"),
                ArrivalTime = f.ArrivalTime.ToString("o")
            }));

            return response;
        }

        public override Task<GetFlightByIdResponse> GetFlightById(GetFlightByIdRequest request, ServerCallContext context)
        {
            var flight = _flights.FirstOrDefault(f => f.Id == request.Id);

            if (flight == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Flight {request.Id} not found"));

            return Task.FromResult(new GetFlightByIdResponse
            {
                Flight = new Grpc.Flight
                {
                    Id = flight.Id,
                    AircraftNumber = flight.AircraftNumber,
                    DepartureAirportCode = flight.DepartureAirportCode,
                    ArrivalAirportCode = flight.ArrivalAirportCode,
                    DepartureCity = flight.DepartureCity,
                    ArrivalCity = flight.ArrivalCity,
                    DepartureTime = flight.DepartureTime.ToString("o"),
                    ArrivalTime = flight.ArrivalTime.ToString("o")
                }
            });
        }

        /// <summary>
        /// Costruisce la schedulazione mock sopra il catalogo condiviso
        /// (TechInterview.SeedMock): aeroporti e compagnie non sono definiti qui,
        /// così i codici aeroporto dei voli esistono sempre in AirportsService.
        /// </summary>
        private static List<FlightsService.Models.Flight> GenerateMockFlights(int count)
        {
            var airports = AirportSeedCatalog.All;
            var flights = new List<FlightsService.Models.Flight>(count);
            var usedFlightCodes = new HashSet<string>(StringComparer.Ordinal);

            // I voli partono dalla mezzanotte UTC di oggi e coprono la finestra di schedulazione.
            var scheduleStart = DateTime.UtcNow.Date;

            while (flights.Count < count)
            {
                var departure = airports[Rand.Next(airports.Count)];
                var arrival = airports[Rand.Next(airports.Count)];

                if (!IsPlausibleRoute(departure, arrival, out var distanceKm))
                    continue;

                var airline = AirlineSeedCatalog.ForCountry(departure.Country);
                var flightCode = $"{airline.Code}{Rand.Next(100, 10000)}";
                if (!usedFlightCodes.Add(flightCode))
                    continue;

                // Orari allineati a 5 minuti, come una schedulazione reale.
                var departureTime = scheduleStart.AddMinutes(5 * Rand.Next(0, ScheduleWindowDays * 24 * 12));

                flights.Add(new FlightsService.Models.Flight
                {
                    Id = flightCode,
                    AircraftNumber = AircraftRegistration.Build(airline, Rand),
                    DepartureAirportCode = departure.Code,
                    ArrivalAirportCode = arrival.Code,
                    DepartureCity = departure.City,
                    ArrivalCity = arrival.City,
                    DepartureTime = departureTime,
                    ArrivalTime = departureTime.AddMinutes(BlockTimeMinutes(distanceKm))
                });
            }

            // Ordine stabile per orario di partenza: la paginazione è ripetibile e leggibile.
            return flights
                .OrderBy(f => f.DepartureTime)
                .ThenBy(f => f.Id, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Scarta le tratte che nella realtà non esistono: stessa città (es. MXP -> LIN),
        /// distanze troppo brevi (es. TSF -> VRN), distanze oltre il raggio di un nonstop
        /// (es. SYD -> LIS) e long haul da o per aeroporti non hub (es. CTA -> SYD).
        /// </summary>
        private static bool IsPlausibleRoute(AirportSeed departure, AirportSeed arrival, out double distanceKm)
        {
            distanceKm = Geo.DistanceKm(departure, arrival);

            if (string.Equals(departure.City, arrival.City, StringComparison.Ordinal))
                return false;

            if (distanceKm < MinRouteDistanceKm || distanceKm > MaxNonstopRangeKm)
                return false;

            return distanceKm <= LongHaulThresholdKm || (departure.IsHub && arrival.IsHub);
        }

        /// <summary>
        /// Durata del volo derivata dalla distanza: rullaggio/salita/discesa più
        /// crociera a velocità costante, arrotondata a 5 minuti.
        /// </summary>
        private static int BlockTimeMinutes(double distanceKm)
        {
            var minutes = GroundTimeMinutes + distanceKm / CruiseSpeedKmh * 60;
            return (int)(Math.Ceiling(minutes / 5) * 5);
        }
    }
}
