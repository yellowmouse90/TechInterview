using AirportsService.Models;
using TechInterview.SeedMock;

namespace AirportsService.Services;

public interface IAirportsRepository
{
    Task<PagedList<Airport>> GetPagedAsync(int offset, int limit);
    Task<Airport?> GetAsync(string id);
}

public class AirportsRepository : IAirportsRepository
{
    private readonly List<Airport> _airports;

    public AirportsRepository()
    {
        _airports = BuildAirports();
    }

    public async Task<PagedList<Airport>> GetPagedAsync(int offset, int limit)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(100 + 50 * limit));
        return new PagedList<Airport>(_airports
            .Skip(offset)
            .Take(limit), _airports.Count);
    }

    public Task<Airport?> GetAsync(string id)
        => Task.FromResult(_airports.SingleOrDefault(x=>string.Equals(x.Id, id, StringComparison.CurrentCultureIgnoreCase)));

    /// <summary>
    /// Proietta il catalogo condiviso (TechInterview.SeedMock) sul modello di dominio:
    /// l'Id è il codice IATA, la stessa chiave con cui i voli referenziano gli aeroporti.
    /// </summary>
    private static List<Airport> BuildAirports() =>
        AirportSeedCatalog.All
            .Select(seed => new Airport
            {
                Id = seed.Code,
                Name = seed.Name,
                City = seed.City,
                Country = seed.Country
            })
            .ToList();
}
