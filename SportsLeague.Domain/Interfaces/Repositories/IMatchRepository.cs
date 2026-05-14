using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchRepository
{
    Task<Match> CreateAsync(Match match);
    Task<Match?> GetByIdAsync(int id);
    Task<Match?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Match>> GetAllAsync();
    Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId);
    Task<IEnumerable<Match>> GetByTournamentWithDetailsAsync(int tournamentId);
    Task<IEnumerable<Match>> GetByTeamAsync(int teamId);
    Task UpdateAsync(Match match);
    Task DeleteAsync(int id);
}