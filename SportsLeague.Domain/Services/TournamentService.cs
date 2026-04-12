using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentTeamRepository _tournamentTeamRepository;
    private readonly ITeamRepository _teamRepository;

    public TournamentService(
        ITournamentRepository tournamentRepository,
        ITournamentTeamRepository tournamentTeamRepository,
        ITeamRepository teamRepository)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentTeamRepository = tournamentTeamRepository;
        _teamRepository = teamRepository;
    }

    public async Task<IEnumerable<Tournament>> GetAllAsync()
        => await _tournamentRepository.GetAllAsync();

    public async Task<Tournament?> GetByIdAsync(int id)
        => await _tournamentRepository.GetWithTeamsAsync(id);

    public async Task<Tournament> CreateAsync(Tournament tournament)
    {
        if (tournament.EndDate <= tournament.StartDate)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        var duplicate = await _tournamentRepository.NameExistsInSeasonAsync(tournament.Name, tournament.Season);
        if (duplicate)
            throw new InvalidOperationException($"Ya existe un torneo '{tournament.Name}' en la temporada '{tournament.Season}'.");

        return await _tournamentRepository.CreateAsync(tournament);
    }

    public async Task UpdateAsync(int id, Tournament tournament)
    {
        var existing = await _tournamentRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Torneo con ID {id} no encontrado.");

        if (existing.Status == TournamentStatus.Finished)
            throw new InvalidOperationException("No se puede modificar un torneo finalizado.");

        if (tournament.EndDate <= tournament.StartDate)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        var duplicate = await _tournamentRepository.NameExistsInSeasonAsync(tournament.Name, tournament.Season, id);
        if (duplicate)
            throw new InvalidOperationException($"Ya existe otro torneo '{tournament.Name}' en la temporada '{tournament.Season}'.");

        existing.Name = tournament.Name;
        existing.Season = tournament.Season;
        existing.StartDate = tournament.StartDate;
        existing.EndDate = tournament.EndDate;
        existing.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);
        if (tournament == null)
            throw new KeyNotFoundException($"Torneo con ID {id} no encontrado.");

        if (tournament.Status == TournamentStatus.Finished)
            throw new InvalidOperationException("No se puede eliminar un torneo finalizado.");

        await _tournamentRepository.DeleteAsync(tournament);
    }

    public async Task UpdateStatusAsync(int id, TournamentStatus newStatus)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);
        if (tournament == null)
            throw new KeyNotFoundException($"Torneo con ID {id} no encontrado.");

        // Máquina de estados: solo transiciones válidas
        var validTransition = (tournament.Status, newStatus) switch
        {
            (TournamentStatus.Pending, TournamentStatus.InProgress) => true,
            (TournamentStatus.InProgress, TournamentStatus.Finished) => true,
            _ => false
        };

        if (!validTransition)
            throw new InvalidOperationException(
                $"Transición de estado inválida: {tournament.Status} → {newStatus}. " +
                "Solo se permite: Pending → InProgress → Finished.");

        tournament.Status = newStatus;
        tournament.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(tournament);
    }

    public async Task RegisterTeamAsync(int tournamentId, int teamId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null)
            throw new KeyNotFoundException($"Torneo con ID {tournamentId} no encontrado.");

        if (tournament.Status != TournamentStatus.Pending)
            throw new InvalidOperationException("Solo se pueden inscribir equipos en torneos con estado Pending.");

        var teamExists = await _teamRepository.ExistsAsync(teamId);
        if (!teamExists)
            throw new KeyNotFoundException($"Equipo con ID {teamId} no encontrado.");

        var alreadyRegistered = await _tournamentTeamRepository.IsTeamRegisteredAsync(tournamentId, teamId);
        if (alreadyRegistered)
            throw new InvalidOperationException($"El equipo {teamId} ya está inscrito en este torneo.");

        var registration = new TournamentTeam
        {
            TournamentId = tournamentId,
            TeamId = teamId,
            RegisteredAt = DateTime.UtcNow
        };

        await _tournamentTeamRepository.CreateAsync(registration);
    }

    public async Task<IEnumerable<Team>> GetTeamsByTournamentAsync(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null)
            throw new KeyNotFoundException($"Torneo con ID {tournamentId} no encontrado.");

        return await _tournamentTeamRepository.GetTeamsByTournamentAsync(tournamentId);
    }
}
