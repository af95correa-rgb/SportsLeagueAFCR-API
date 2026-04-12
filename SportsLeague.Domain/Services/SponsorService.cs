using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentSponsorRepository _tsRepository;

    public SponsorService(
        ISponsorRepository sponsorRepository,
        ITournamentRepository tournamentRepository,
        ITournamentSponsorRepository tsRepository)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentRepository = tournamentRepository;
        _tsRepository = tsRepository;
    }

    public async Task<IEnumerable<Sponsor>> GetAllAsync()
        => await _sponsorRepository.GetAllAsync();

    public async Task<Sponsor?> GetByIdAsync(int id)
        => await _sponsorRepository.GetByIdAsync(id);

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
            throw new InvalidOperationException("Ya existe un sponsor con ese nombre.");

        if (!IsValidEmail(sponsor.ContactEmail))
            throw new InvalidOperationException("Email inválido.");

        return await _sponsorRepository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existing = await _sponsorRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado.");

        if (!IsValidEmail(sponsor.ContactEmail))
            throw new InvalidOperationException("Email inválido.");

        existing.Name = sponsor.Name;
        existing.ContactEmail = sponsor.ContactEmail;
        existing.Phone = sponsor.Phone;
        existing.WebsiteUrl = sponsor.WebsiteUrl;
        existing.Category = sponsor.Category;
        existing.UpdatedAt = DateTime.UtcNow;

        await _sponsorRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(id);
        if (sponsor == null)
            throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado.");

        await _sponsorRepository.DeleteAsync(sponsor);
    }

    // 🔗 N:M

    public async Task LinkSponsorAsync(int sponsorId, int tournamentId, decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("El monto debe ser mayor a 0.");

        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
        if (sponsor == null)
            throw new KeyNotFoundException("Sponsor no existe.");

        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null)
            throw new KeyNotFoundException("Tournament no existe.");

        var exists = await _tsRepository.ExistsAsync(tournamentId, sponsorId);
        if (exists)
            throw new InvalidOperationException("Ya está vinculado.");

        var entity = new TournamentSponsor
        {
            SponsorId = sponsorId,
            TournamentId = tournamentId,
            ContractAmount = amount,
            JoinedAt = DateTime.UtcNow
        };

        await _tsRepository.CreateAsync(entity);
    }

    public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
        if (sponsor == null)
            throw new KeyNotFoundException("Sponsor no existe.");

        return await _tsRepository.GetBySponsorIdAsync(sponsorId);
    }

    public async Task UnlinkSponsorAsync(int sponsorId, int tournamentId)
    {
        var list = await _tsRepository.GetBySponsorIdAsync(sponsorId);
        var entity = list.FirstOrDefault(x => x.TournamentId == tournamentId);

        if (entity == null)
            throw new KeyNotFoundException("Relación no existe.");

        await _tsRepository.DeleteAsync(entity);
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}