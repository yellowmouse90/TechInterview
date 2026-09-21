namespace TechInterview.SeedMock
{
    /// <summary>Calcoli geografici sugli aeroporti del catalogo.</summary>
    public static class Geo
    {
        private const double EarthRadiusKm = 6371;

        /// <summary>Distanza ortodromica in chilometri (formula dell'emisenoverso).</summary>
        public static double DistanceKm(AirportSeed from, AirportSeed to)
        {
            var dLat = double.DegreesToRadians(to.Latitude - from.Latitude);
            var dLon = double.DegreesToRadians(to.Longitude - from.Longitude);
            var lat1 = double.DegreesToRadians(from.Latitude);
            var lat2 = double.DegreesToRadians(to.Latitude);

            var a = Math.Pow(Math.Sin(dLat / 2), 2)
                    + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);

            return EarthRadiusKm * 2 * Math.Asin(Math.Sqrt(a));
        }
    }
}
