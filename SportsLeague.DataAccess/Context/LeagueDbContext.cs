using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
        : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Referee> Referees { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<TournamentTeam> TournamentTeams { get; set; }
    public DbSet<TournamentSponsor> TournamentSponsors { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchResult> MatchResults { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>()
            .HasIndex(t => t.Name)
            .IsUnique();

        modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TournamentTeam>()
            .HasOne(tt => tt.Tournament)
            .WithMany(t => t.TournamentTeams)
            .HasForeignKey(tt => tt.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TournamentTeam>()
            .HasOne(tt => tt.Team)
            .WithMany(t => t.TournamentTeams)
            .HasForeignKey(tt => tt.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TournamentTeam>()
            .HasIndex(tt => new { tt.TournamentId, tt.TeamId })
            .IsUnique();

        modelBuilder.Entity<Sponsor>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<TournamentSponsor>()
            .HasOne(ts => ts.Tournament)
            .WithMany(t => t.TournamentSponsors)
            .HasForeignKey(ts => ts.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TournamentSponsor>()
            .HasOne(ts => ts.Sponsor)
            .WithMany(s => s.TournamentSponsors)
            .HasForeignKey(ts => ts.SponsorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TournamentSponsor>()
            .HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
            .IsUnique();

        modelBuilder.Entity<TournamentSponsor>()
            .Property(ts => ts.ContractAmount)
            .HasPrecision(18, 2);

        // ── Match Configuration ──
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.MatchDate).IsRequired();
            entity.Property(m => m.Venue).HasMaxLength(150);
            entity.Property(m => m.Matchday).IsRequired();
            entity.Property(m => m.Status).IsRequired();
            entity.Property(m => m.CreatedAt).IsRequired();
            entity.Property(m => m.UpdatedAt).IsRequired(false);

            // Cascade: eliminar torneo elimina sus partidos
            entity.HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restrict: evita ciclo de cascada desde Team
            entity.HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict: no eliminar árbitro con partidos
            entity.HasOne(m => m.Referee)
                .WithMany(r => r.Matches)
                .HasForeignKey(m => m.RefereeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── MatchResult Configuration ──
        modelBuilder.Entity<MatchResult>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            entity.Property(mr => mr.HomeGoals).IsRequired();
            entity.Property(mr => mr.AwayGoals).IsRequired();
            entity.Property(mr => mr.Observations).HasMaxLength(500);
            entity.Property(mr => mr.CreatedAt).IsRequired();
            entity.Property(mr => mr.UpdatedAt).IsRequired(false);

            entity.HasOne(mr => mr.Match)
                .WithOne(m => m.MatchResult)
                .HasForeignKey<MatchResult>(mr => mr.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(mr => mr.MatchId).IsUnique();
        });

        // ── Goal Configuration ──
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Minute).IsRequired();
            entity.Property(g => g.Type).IsRequired();
            entity.Property(g => g.CreatedAt).IsRequired();
            entity.Property(g => g.UpdatedAt).IsRequired(false);

            entity.HasOne(g => g.Match)
                .WithMany(m => m.Goals)
                .HasForeignKey(g => g.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Player)
                .WithMany(p => p.Goals)
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Card Configuration ──
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Minute).IsRequired();
            entity.Property(c => c.Type).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.UpdatedAt).IsRequired(false);

            entity.HasOne(c => c.Match)
                .WithMany(m => m.Cards)
                .HasForeignKey(c => c.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Player)
                .WithMany(p => p.Cards)
                .HasForeignKey(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });


    }
}