using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
{
    public PlayerRepository(LeagueDbContext context) : base(context) { }

    public new async Task<IEnumerable<Player>> GetAllAsync()
        => await _dbSet.Include(p => p.Team).ToListAsync();

    public new async Task<Player?> GetByIdAsync(int id)
        => await _dbSet.Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId)
        => await _dbSet
            .Include(p => p.Team)
            .Where(p => p.TeamId == teamId)
            .ToListAsync();

    public async Task<bool> NumberExistsInTeamAsync(int teamId, int number, int? excludePlayerId = null)
    {
        var query = _dbSet.Where(p => p.TeamId == teamId && p.Number == number);
        if (excludePlayerId.HasValue)
            query = query.Where(p => p.Id != excludePlayerId.Value);
        return await query.AnyAsync();
    }
}
