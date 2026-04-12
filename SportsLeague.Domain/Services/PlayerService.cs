using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamRepository _teamRepository;

    public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
    {
        _playerRepository = playerRepository;
        _teamRepository = teamRepository;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
        => await _playerRepository.GetAllAsync();

    public async Task<Player?> GetByIdAsync(int id)
        => await _playerRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId)
    {
        var teamExists = await _teamRepository.ExistsAsync(teamId);
        if (!teamExists)
            throw new KeyNotFoundException($"Equipo con ID {teamId} no encontrado.");

        return await _playerRepository.GetByTeamIdAsync(teamId);
    }

    public async Task<Player> CreateAsync(Player player)
    {
        var teamExists = await _teamRepository.ExistsAsync(player.TeamId);
        if (!teamExists)
            throw new KeyNotFoundException($"Equipo con ID {player.TeamId} no encontrado.");

        var numberTaken = await _playerRepository.NumberExistsInTeamAsync(player.TeamId, player.Number);
        if (numberTaken)
            throw new InvalidOperationException($"El número {player.Number} ya está asignado en este equipo.");

        return await _playerRepository.CreateAsync(player);
    }

    public async Task UpdateAsync(int id, Player player)
    {
        var existing = await _playerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Jugador con ID {id} no encontrado.");

        var teamExists = await _teamRepository.ExistsAsync(player.TeamId);
        if (!teamExists)
            throw new KeyNotFoundException($"Equipo con ID {player.TeamId} no encontrado.");

        var numberTaken = await _playerRepository.NumberExistsInTeamAsync(player.TeamId, player.Number, id);
        if (numberTaken)
            throw new InvalidOperationException($"El número {player.Number} ya está asignado en este equipo.");

        existing.FirstName = player.FirstName;
        existing.LastName = player.LastName;
        existing.BirthDate = player.BirthDate;
        existing.Number = player.Number;
        existing.Position = player.Position;
        existing.TeamId = player.TeamId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _playerRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player == null)
            throw new KeyNotFoundException($"Jugador con ID {id} no encontrado.");

        await _playerRepository.DeleteAsync(player);
    }
}
