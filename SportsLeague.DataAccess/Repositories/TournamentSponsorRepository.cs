using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
{
    public TournamentSponsorRepository(LeagueDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(int tournamentId, int sponsorId)
        => await _dbSet.AnyAsync(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId);

    public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        => await _dbSet
            .Include(ts => ts.Tournament)
            .Where(ts => ts.SponsorId == sponsorId)
            .ToListAsync();
}