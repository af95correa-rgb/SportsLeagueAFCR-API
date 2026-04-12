using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options) : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Referee> Referees { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentTeam> TournamentTeams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Team: nombre único
        modelBuilder.Entity<Team>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // Player → Team (1:N) — si se elimina equipo, restringir
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // TournamentTeam → Tournament (1:N)
        modelBuilder.Entity<TournamentTeam>()
            .HasOne(tt => tt.Tournament)
            .WithMany(t => t.TournamentTeams)
            .HasForeignKey(tt => tt.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        // TournamentTeam → Team (1:N)
        modelBuilder.Entity<TournamentTeam>()
            .HasOne(tt => tt.Team)
            .WithMany(t => t.TournamentTeams)
            .HasForeignKey(tt => tt.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice único compuesto: un equipo solo puede inscribirse una vez por torneo
        modelBuilder.Entity<TournamentTeam>()
            .HasIndex(tt => new { tt.TournamentId, tt.TeamId })
            .IsUnique();
    }
}
