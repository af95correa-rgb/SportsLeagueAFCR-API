using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    Task<bool> ExistsAsync(int tournamentId, int sponsorId);
    Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
}