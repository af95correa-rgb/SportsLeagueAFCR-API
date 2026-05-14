using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentTeamRepository : IGenericRepository<TournamentTeam>
{
    Task<bool> IsTeamRegisteredAsync(int tournamentId, int teamId);
    Task<IEnumerable<Team>> GetTeamsByTournamentAsync(int tournamentId);
    Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId); // ← NUEVO
}