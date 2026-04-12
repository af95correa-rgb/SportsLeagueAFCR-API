using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(LeagueDbContext context) : base(context) { }

    public async Task<Team?> GetByNameAsync(string name)
        => await _dbSet.FirstOrDefaultAsync(t => t.Name == name);

    public async Task<bool> ExistsAsync(int id)
        => await _dbSet.AnyAsync(t => t.Id == id);
}
