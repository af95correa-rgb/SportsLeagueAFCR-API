using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _lineupRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchLineupService> _logger;

    public MatchLineupService(
        IMatchLineupRepository lineupRepository,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchLineupService> logger)
    {
        _lineupRepository = lineupRepository;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<MatchLineup> AddPlayerAsync(int matchId, MatchLineup lineup)
    {
        // V1: El partido existe
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

        // V6: El partido debe estar en Programado
        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden registrar alineaciones en partidos Programado");

        // V2: El jugador existe
        var player = await _playerRepository.GetByIdAsync(lineup.PlayerId);
        if (player == null)
            throw new KeyNotFoundException($"No se encontró el jugador con ID {lineup.PlayerId}");

        // V3: El jugador pertenece al HomeTeam o AwayTeam
        if (player.TeamId != match.HomeTeamId && player.TeamId != match.AwayTeamId)
            throw new InvalidOperationException(
                "El jugador no pertenece a ninguno de los equipos del partido");

        // V4: El jugador no está duplicado en la alineación
        var alreadyExists = await _lineupRepository
            .ExistsByMatchAndPlayerAsync(matchId, lineup.PlayerId);
        if (alreadyExists)
            throw new InvalidOperationException(
                "El jugador ya está registrado en la alineación de este partido");

        // V5: Máximo 11 titulares por equipo
        if (lineup.IsStarter)
        {
            var starterCount = await _lineupRepository
                .CountStartersByMatchAndTeamAsync(matchId, player.TeamId);
            if (starterCount >= 11)
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 titulares registrados en este partido");
        }

        lineup.MatchId = matchId;

        _logger.LogInformation(
            "Agregando Jugador {PlayerId} a formación del partido {MatchId}", lineup.PlayerId, matchId);

        return await _lineupRepository.CreateAsync(lineup);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

        return await _lineupRepository.GetByMatchAsync(matchId);
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

        if (match.HomeTeamId != teamId && match.AwayTeamId != teamId)
            throw new InvalidOperationException(
                "El equipo no participa en este partido");

        return await _lineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
    }

    public async Task DeleteAsync(int lineupId)
    {
        var lineup = await _lineupRepository.GetByIdAsync(lineupId);
        if (lineup == null)
            throw new KeyNotFoundException($"No se encontró el registro de alineación con ID {lineupId}");

        _logger.LogInformation("Borrando alinación {LineupId}", lineupId);
        await _lineupRepository.DeleteAsync(lineup);
    }
}