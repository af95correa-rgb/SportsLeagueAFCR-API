using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
        => await _teamRepository.GetAllAsync();

    public async Task<Team?> GetByIdAsync(int id)
        => await _teamRepository.GetByIdAsync(id);

    public async Task<Team> CreateAsync(Team team)
    {
        var existing = await _teamRepository.GetByNameAsync(team.Name);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe un equipo con el nombre '{team.Name}'.");

        return await _teamRepository.CreateAsync(team);
    }

    public async Task UpdateAsync(int id, Team team)
    {
        var existing = await _teamRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        var duplicate = await _teamRepository.GetByNameAsync(team.Name);
        if (duplicate != null && duplicate.Id != id)
            throw new InvalidOperationException($"Ya existe otro equipo con el nombre '{team.Name}'.");

        existing.Name = team.Name;
        existing.City = team.City;
        existing.Stadium = team.Stadium;
        existing.LogoUrl = team.LogoUrl;
        existing.FoundedDate = team.FoundedDate;
        existing.UpdatedAt = DateTime.UtcNow;

        await _teamRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
            throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        await _teamRepository.DeleteAsync(team);
    }
}
