namespace TechInterview.SeedMock
{
    /// <summary>
    /// Catalogo condiviso degli aeroporti (dati reali, coordinate approssimate).
    /// È l'unica definizione presente nella solution: AirportsService lo espone come
    /// risorsa REST, FlightsService lo usa come rete di rotte. Aggiungere un aeroporto
    /// qui lo rende disponibile a entrambi i domini, senza rischio di disallineamento.
    /// </summary>
    public static class AirportSeedCatalog
    {
        private static readonly AirportSeed[] Entries =
        {
            // Italia
            new("MXP", "Milano Malpensa", "Milano", "Italy", 45.630, 8.723, IsHub: true),
            new("LIN", "Milano Linate", "Milano", "Italy", 45.445, 9.277, IsHub: false),
            new("BGY", "Milano Bergamo Orio al Serio", "Bergamo", "Italy", 45.669, 9.704, IsHub: false),
            new("FCO", "Roma Fiumicino Leonardo da Vinci", "Roma", "Italy", 41.800, 12.239, IsHub: true),
            new("CIA", "Roma Ciampino Giovan Battista Pastine", "Roma", "Italy", 41.799, 12.595, IsHub: false),
            new("FLR", "Firenze Peretola Amerigo Vespucci", "Firenze", "Italy", 43.810, 11.205, IsHub: false),
            new("PSA", "Pisa Galileo Galilei", "Pisa", "Italy", 43.684, 10.393, IsHub: false),
            new("BLQ", "Bologna Guglielmo Marconi", "Bologna", "Italy", 44.535, 11.289, IsHub: false),
            new("VCE", "Venezia Marco Polo", "Venezia", "Italy", 45.505, 12.352, IsHub: false),
            new("TSF", "Treviso Antonio Canova", "Treviso", "Italy", 45.648, 12.194, IsHub: false),
            new("VRN", "Verona Valerio Catullo", "Verona", "Italy", 45.396, 10.888, IsHub: false),
            new("TRN", "Torino Caselle Sandro Pertini", "Torino", "Italy", 45.201, 7.650, IsHub: false),
            new("GOA", "Genova Cristoforo Colombo", "Genova", "Italy", 44.413, 8.837, IsHub: false),
            new("NAP", "Napoli Capodichino", "Napoli", "Italy", 40.886, 14.291, IsHub: false),
            new("BRI", "Bari Karol Wojtyla", "Bari", "Italy", 41.139, 16.760, IsHub: false),
            new("CTA", "Catania Fontanarossa Vincenzo Bellini", "Catania", "Italy", 37.467, 15.066, IsHub: false),
            new("PMO", "Palermo Falcone Borsellino", "Palermo", "Italy", 38.176, 13.091, IsHub: false),
            new("CAG", "Cagliari Elmas", "Cagliari", "Italy", 39.251, 9.054, IsHub: false),

            // Francia
            new("CDG", "Paris Charles de Gaulle", "Paris", "France", 49.010, 2.548, IsHub: true),
            new("ORY", "Paris Orly", "Paris", "France", 48.726, 2.366, IsHub: false),
            new("NCE", "Nice Cote d'Azur", "Nice", "France", 43.665, 7.215, IsHub: false),
            new("LYS", "Lyon Saint-Exupery", "Lyon", "France", 45.726, 5.091, IsHub: false),
            new("MRS", "Marseille Provence", "Marseille", "France", 43.436, 5.215, IsHub: false),
            new("TLS", "Toulouse Blagnac", "Toulouse", "France", 43.629, 1.364, IsHub: false),

            // Regno Unito
            new("LHR", "London Heathrow", "London", "United Kingdom", 51.470, -0.454, IsHub: true),
            new("LGW", "London Gatwick", "London", "United Kingdom", 51.148, -0.190, IsHub: true),
            new("STN", "London Stansted", "London", "United Kingdom", 51.885, 0.235, IsHub: false),
            new("MAN", "Manchester", "Manchester", "United Kingdom", 53.354, -2.275, IsHub: true),
            new("EDI", "Edinburgh", "Edinburgh", "United Kingdom", 55.950, -3.372, IsHub: false),
            new("BHX", "Birmingham", "Birmingham", "United Kingdom", 52.454, -1.748, IsHub: false),

            // Spagna
            new("MAD", "Madrid Barajas Adolfo Suarez", "Madrid", "Spain", 40.472, -3.561, IsHub: true),
            new("BCN", "Barcelona El Prat Josep Tarradellas", "Barcelona", "Spain", 41.297, 2.078, IsHub: true),
            new("AGP", "Malaga Costa del Sol", "Malaga", "Spain", 36.675, -4.499, IsHub: false),
            new("PMI", "Palma de Mallorca", "Palma de Mallorca", "Spain", 39.552, 2.739, IsHub: false),
            new("VLC", "Valencia", "Valencia", "Spain", 39.489, -0.482, IsHub: false),
            new("SVQ", "Sevilla", "Sevilla", "Spain", 37.418, -5.893, IsHub: false),

            // Germania
            new("FRA", "Frankfurt am Main", "Frankfurt", "Germany", 50.038, 8.562, IsHub: true),
            new("MUC", "Munchen Franz Josef Strauss", "Munchen", "Germany", 48.354, 11.786, IsHub: true),
            new("BER", "Berlin Brandenburg Willy Brandt", "Berlin", "Germany", 52.362, 13.501, IsHub: false),
            new("DUS", "Dusseldorf", "Dusseldorf", "Germany", 51.289, 6.767, IsHub: false),
            new("HAM", "Hamburg", "Hamburg", "Germany", 53.630, 9.988, IsHub: false),
            new("STR", "Stuttgart", "Stuttgart", "Germany", 48.690, 9.222, IsHub: false),

            // Resto d'Europa
            new("AMS", "Amsterdam Schiphol", "Amsterdam", "Netherlands", 52.309, 4.764, IsHub: true),
            new("BRU", "Brussels", "Brussels", "Belgium", 50.901, 4.484, IsHub: true),
            new("ZRH", "Zurich", "Zurich", "Switzerland", 47.458, 8.548, IsHub: true),
            new("GVA", "Geneve", "Geneve", "Switzerland", 46.238, 6.109, IsHub: false),
            new("VIE", "Wien Schwechat", "Wien", "Austria", 48.110, 16.570, IsHub: true),
            new("LIS", "Lisboa Humberto Delgado", "Lisboa", "Portugal", 38.774, -9.134, IsHub: true),
            new("OPO", "Porto Francisco Sa Carneiro", "Porto", "Portugal", 41.248, -8.681, IsHub: false),
            new("DUB", "Dublin", "Dublin", "Ireland", 53.427, -6.243, IsHub: true),
            new("CPH", "Kobenhavn Kastrup", "Kobenhavn", "Denmark", 55.618, 12.656, IsHub: true),
            new("ARN", "Stockholm Arlanda", "Stockholm", "Sweden", 59.652, 17.919, IsHub: true),
            new("OSL", "Oslo Gardermoen", "Oslo", "Norway", 60.194, 11.100, IsHub: true),
            new("HEL", "Helsinki Vantaa", "Helsinki", "Finland", 60.317, 24.963, IsHub: true),
            new("WAW", "Warsaw Chopin", "Warsaw", "Poland", 52.166, 20.967, IsHub: true),
            new("PRG", "Prague Vaclav Havel", "Prague", "Czech Republic", 50.101, 14.260, IsHub: false),
            new("BUD", "Budapest Ferenc Liszt", "Budapest", "Hungary", 47.437, 19.256, IsHub: false),
            new("ATH", "Athens Eleftherios Venizelos", "Athens", "Greece", 37.936, 23.945, IsHub: true),
            new("IST", "Istanbul", "Istanbul", "Turkey", 41.262, 28.742, IsHub: true),
            new("OTP", "Bucharest Henri Coanda", "Bucharest", "Romania", 44.572, 26.102, IsHub: false),

            // Nord e Sud America
            new("JFK", "New York John F. Kennedy", "New York", "United States", 40.641, -73.778, IsHub: true),
            new("LGA", "New York LaGuardia", "New York", "United States", 40.777, -73.872, IsHub: false),
            new("EWR", "Newark Liberty", "Newark", "United States", 40.689, -74.175, IsHub: true),
            new("BOS", "Boston Logan", "Boston", "United States", 42.366, -71.020, IsHub: true),
            new("IAD", "Washington Dulles", "Washington", "United States", 38.953, -77.456, IsHub: true),
            new("ORD", "Chicago O'Hare", "Chicago", "United States", 41.979, -87.904, IsHub: true),
            new("ATL", "Atlanta Hartsfield-Jackson", "Atlanta", "United States", 33.641, -84.427, IsHub: true),
            new("MIA", "Miami", "Miami", "United States", 25.795, -80.287, IsHub: true),
            new("DFW", "Dallas Fort Worth", "Dallas", "United States", 32.897, -97.040, IsHub: true),
            new("DEN", "Denver", "Denver", "United States", 39.862, -104.673, IsHub: true),
            new("LAS", "Las Vegas Harry Reid", "Las Vegas", "United States", 36.084, -115.154, IsHub: true),
            new("LAX", "Los Angeles", "Los Angeles", "United States", 33.942, -118.408, IsHub: true),
            new("SFO", "San Francisco", "San Francisco", "United States", 37.619, -122.375, IsHub: true),
            new("SEA", "Seattle Tacoma", "Seattle", "United States", 47.450, -122.309, IsHub: true),
            new("YYZ", "Toronto Pearson", "Toronto", "Canada", 43.677, -79.625, IsHub: true),
            new("YUL", "Montreal Pierre Elliott Trudeau", "Montreal", "Canada", 45.470, -73.741, IsHub: true),
            new("YVR", "Vancouver", "Vancouver", "Canada", 49.194, -123.184, IsHub: true),
            new("MEX", "Ciudad de Mexico Benito Juarez", "Ciudad de Mexico", "Mexico", 19.436, -99.072, IsHub: true),
            new("GRU", "Sao Paulo Guarulhos", "Sao Paulo", "Brazil", -23.435, -46.473, IsHub: true),
            new("EZE", "Buenos Aires Ezeiza Ministro Pistarini", "Buenos Aires", "Argentina", -34.822, -58.536, IsHub: true),

            // Medio Oriente e Africa
            new("DXB", "Dubai", "Dubai", "United Arab Emirates", 25.253, 55.365, IsHub: true),
            new("AUH", "Abu Dhabi Zayed", "Abu Dhabi", "United Arab Emirates", 24.433, 54.651, IsHub: true),
            new("DOH", "Doha Hamad", "Doha", "Qatar", 25.273, 51.608, IsHub: true),
            new("TLV", "Tel Aviv Ben Gurion", "Tel Aviv", "Israel", 32.011, 34.887, IsHub: true),
            new("CAI", "Cairo", "Cairo", "Egypt", 30.122, 31.406, IsHub: true),
            new("CMN", "Casablanca Mohammed V", "Casablanca", "Morocco", 33.367, -7.590, IsHub: true),
            new("JNB", "Johannesburg O. R. Tambo", "Johannesburg", "South Africa", -26.139, 28.246, IsHub: true),
            new("CPT", "Cape Town", "Cape Town", "South Africa", -33.969, 18.602, IsHub: true),
            new("NBO", "Nairobi Jomo Kenyatta", "Nairobi", "Kenya", -1.319, 36.928, IsHub: true),

            // Asia e Oceania
            new("HND", "Tokyo Haneda", "Tokyo", "Japan", 35.549, 139.780, IsHub: true),
            new("NRT", "Tokyo Narita", "Tokyo", "Japan", 35.765, 140.386, IsHub: true),
            new("ICN", "Seoul Incheon", "Seoul", "South Korea", 37.469, 126.451, IsHub: true),
            new("PEK", "Beijing Capital", "Beijing", "China", 40.080, 116.585, IsHub: true),
            new("PVG", "Shanghai Pudong", "Shanghai", "China", 31.144, 121.805, IsHub: true),
            new("HKG", "Hong Kong", "Hong Kong", "Hong Kong", 22.308, 113.915, IsHub: true),
            new("TPE", "Taipei Taoyuan", "Taipei", "Taiwan", 25.077, 121.233, IsHub: true),
            new("SIN", "Singapore Changi", "Singapore", "Singapore", 1.364, 103.991, IsHub: true),
            new("BKK", "Bangkok Suvarnabhumi", "Bangkok", "Thailand", 13.690, 100.750, IsHub: true),
            new("KUL", "Kuala Lumpur", "Kuala Lumpur", "Malaysia", 2.746, 101.710, IsHub: true),
            new("CGK", "Jakarta Soekarno-Hatta", "Jakarta", "Indonesia", -6.126, 106.656, IsHub: true),
            new("DEL", "Delhi Indira Gandhi", "Delhi", "India", 28.556, 77.100, IsHub: true),
            new("BOM", "Mumbai Chhatrapati Shivaji", "Mumbai", "India", 19.089, 72.868, IsHub: true),
            new("SYD", "Sydney Kingsford Smith", "Sydney", "Australia", -33.946, 151.177, IsHub: true),
            new("MEL", "Melbourne", "Melbourne", "Australia", -37.669, 144.841, IsHub: true),
            new("AKL", "Auckland", "Auckland", "New Zealand", -37.008, 174.792, IsHub: true)
        };

        /// <summary>
        /// Tutti gli aeroporti, ordinati per codice IATA: l'ordine è stabile tra
        /// chiamate e riavvii, così la paginazione è ripetibile.
        /// </summary>
        public static IReadOnlyList<AirportSeed> All { get; } =
            Entries.OrderBy(entry => entry.Code, StringComparer.Ordinal).ToArray();
    }
}
