using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentRepository : IGenericRepository<Tournament>
{
    Task<Tournament?> GetWithTeamsAsync(int id);
    Task<bool> NameExistsInSeasonAsync(string name, string season, int? excludeId = null);
}
