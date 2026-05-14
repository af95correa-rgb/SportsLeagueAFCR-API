namespace SportsLeague.Domain.Entities;

public class MatchResult : AuditBase
{
    public int MatchId { get; set; }
    public int HomeGoals { get; set; }      // era HomeScore
    public int AwayGoals { get; set; }      // era AwayScore
    public string? Observations { get; set; } // NUEVO

    public Match Match { get; set; } = null!;
}