using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentTeamRepository : GenericRepository<TournamentTeam>, ITournamentTeamRepository
{
    public TournamentTeamRepository(LeagueDbContext context) : base(context) { }

    public async Task<bool> IsTeamRegisteredAsync(int tournamentId, int teamId)
        => await _dbSet.AnyAsync(tt => tt.TournamentId == tournamentId && tt.TeamId == teamId);

    public async Task<IEnumerable<Team>> GetTeamsByTournamentAsync(int tournamentId)
        => await _dbSet
            .Where(tt => tt.TournamentId == tournamentId)
            .Include(tt => tt.Team)
            .Select(tt => tt.Team)
            .ToListAsync();

    public async Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId)
        => await _dbSet.FirstOrDefaultAsync(tt =>
            tt.TournamentId == tournamentId && tt.TeamId == teamId);

    public async Task<IEnumerable<TournamentTeam>> GetByTournamentAsync(int tournamentId) // ← NUEVA
        => await _dbSet
            .Where(tt => tt.TournamentId == tournamentId)
            .Include(tt => tt.Team)
            .ToListAsync();
}