namespace SportsLeague.Domain.Entities;

public class MatchResult : AuditBase
{
    public int MatchId { get; set; }

    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public Match Match { get; set; } = null!;
}