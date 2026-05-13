using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchService
{
    Task<IEnumerable<Match>> GetAllAsync();
    Task<Match?> GetByIdAsync(int id);
    Task<Match> CreateMatchAsync(Match match);
    Task UpdateStatusAsync(int id, MatchStatus status);
}