using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IPlayerRepository : IGenericRepository<Player>
{
    Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId);
    Task<bool> NumberExistsInTeamAsync(int teamId, int number, int? excludePlayerId = null);
}
