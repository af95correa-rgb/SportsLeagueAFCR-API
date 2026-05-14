using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly LeagueDbContext _context;

    public MatchRepository(LeagueDbContext context)
    {
        _context = context;
    }

    public async Task<Match> CreateAsync(Match match)
    {
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();
        return match;
    }

    public async Task<Match?> GetByIdAsync(int id)
    {
        return await _context.Matches.FindAsync(id);
    }

    public async Task<Match?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Matches
            .Include(m => m.Tournament)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Referee)
            .Include(m => m.Goals)
            .Include(m => m.Cards)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Match>> GetAllAsync()
    {
        return await _context.Matches
            .Include(m => m.Tournament)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Referee)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId)
    {
        return await _context.Matches
            .Where(m => m.TournamentId == tournamentId)
            .OrderBy(m => m.Matchday)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetByTournamentWithDetailsAsync(int tournamentId)
    {
        return await _context.Matches
            .Where(m => m.TournamentId == tournamentId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Referee)
            .OrderBy(m => m.Matchday)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetByTeamAsync(int teamId)
    {
        return await _context.Matches
            .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .OrderBy(m => m.MatchDate)
            .ToListAsync();
    }

    public async Task UpdateAsync(Match match)
    {
        _context.Matches.Update(match);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match != null)
        {
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }
}