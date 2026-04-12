using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class RefereeService : IRefereeService
{
    private readonly IRefereeRepository _refereeRepository;

    public RefereeService(IRefereeRepository refereeRepository)
    {
        _refereeRepository = refereeRepository;
    }

    public async Task<IEnumerable<Referee>> GetAllAsync()
        => await _refereeRepository.GetAllAsync();

    public async Task<Referee?> GetByIdAsync(int id)
        => await _refereeRepository.GetByIdAsync(id);

    public async Task<Referee> CreateAsync(Referee referee)
        => await _refereeRepository.CreateAsync(referee);

    public async Task UpdateAsync(int id, Referee referee)
    {
        var existing = await _refereeRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Árbitro con ID {id} no encontrado.");

        existing.FirstName = referee.FirstName;
        existing.LastName = referee.LastName;
        existing.Nationality = referee.Nationality;
        existing.UpdatedAt = DateTime.UtcNow;

        await _refereeRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var referee = await _refereeRepository.GetByIdAsync(id);
        if (referee == null)
            throw new KeyNotFoundException($"Árbitro con ID {id} no encontrado.");

        await _refereeRepository.DeleteAsync(referee);
    }
}
