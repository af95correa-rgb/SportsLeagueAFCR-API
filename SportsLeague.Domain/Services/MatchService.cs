using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITournamentTeamRepository _tournamentTeamRepository;

    public MatchService(
        IMatchRepository matchRepository,
        ITournamentTeamRepository tournamentTeamRepository)
    {
        _matchRepository = matchRepository;
        _tournamentTeamRepository = tournamentTeamRepository;
    }

    public async Task<IEnumerable<Match>> GetAllAsync()
        => await _matchRepository.GetAllAsync();

    public async Task<Match?> GetByIdAsync(int id)
        => await _matchRepository.GetByIdAsync(id);

    public async Task<Match> CreateMatchAsync(Match match)
    {
        if (match.HomeTeamId == match.AwayTeamId)
            throw new InvalidOperationException(
                "Un equipo no puede jugar contra sí mismo.");

        var homeRegistered = await _tournamentTeamRepository
            .IsTeamRegisteredAsync(match.TournamentId, match.HomeTeamId);

        var awayRegistered = await _tournamentTeamRepository
            .IsTeamRegisteredAsync(match.TournamentId, match.AwayTeamId);

        if (!homeRegistered || !awayRegistered)
            throw new InvalidOperationException(
                "Ambos equipos deben estar inscritos en el torneo.");

        return await _matchRepository.CreateAsync(match);
    }

    public async Task UpdateStatusAsync(int id, MatchStatus status)
    {
        var match = await _matchRepository.GetByIdAsync(id);

        if (match == null)
            throw new KeyNotFoundException("Partido no encontrado.");

        match.Status = status;
        match.UpdatedAt = DateTime.UtcNow;

        await _matchRepository.UpdateAsync(match);
    }
}