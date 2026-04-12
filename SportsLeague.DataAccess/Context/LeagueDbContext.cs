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
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<TournamentSponsor> TournamentSponsors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Team: nombre único
        modelBuilder.Entity<Team>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // Player → Team (1:N)
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

        // Índice único compuesto (TournamentTeam)
        modelBuilder.Entity<TournamentTeam>()
            .HasIndex(tt => new { tt.TournamentId, tt.TeamId })
            .IsUnique();

        // Sponsor: nombre único
        modelBuilder.Entity<Sponsor>()
            .HasIndex(s => s.Name)
            .IsUnique();

        // TournamentSponsor → Tournament (1:N)
        modelBuilder.Entity<TournamentSponsor>()
            .HasOne(ts => ts.Tournament)
            .WithMany(t => t.TournamentSponsors)
            .HasForeignKey(ts => ts.TournamentId);

        // TournamentSponsor → Sponsor (1:N)
        modelBuilder.Entity<TournamentSponsor>()
            .HasOne(ts => ts.Sponsor)
            .WithMany(s => s.TournamentSponsors)
            .HasForeignKey(ts => ts.SponsorId);

        // Índice único compuesto (TournamentSponsor)
        modelBuilder.Entity<TournamentSponsor>()
            .HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
            .IsUnique();

        // Guarda datos decimales con precisión (TournamentSponsor.ContractAmount)
        modelBuilder.Entity<TournamentSponsor>()
            .Property(ts => ts.ContractAmount)
            .HasPrecision(18, 2);
    }
}