using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
{
    public TournamentRepository(LeagueDbContext context) : base(context) { }

    public async Task<Tournament?> GetWithTeamsAsync(int id)
        => await _dbSet
            .Include(t => t.TournamentTeams)
                .ThenInclude(tt => tt.Team)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> NameExistsInSeasonAsync(string name, string season, int? excludeId = null)
    {
        var query = _dbSet.Where(t => t.Name == name && t.Season == season);
        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);
        return await query.AnyAsync();
    }
}
