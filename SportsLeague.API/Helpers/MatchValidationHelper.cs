using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.Helpers;

public class MatchValidationHelper
{
    public void ValidatePlayerInMatch(Match match, int playerId)
    {
        var isFromHome = match.HomeTeam.Players.Any(p => p.Id == playerId);
        var isFromAway = match.AwayTeam.Players.Any(p => p.Id == playerId);

        if (!isFromHome && !isFromAway)
            throw new InvalidOperationException(
                "El jugador no pertenece a ninguno de los equipos del partido.");
    }

    public void ValidateMatchStatusForEvents(Match match)
    {
        if (match.Status != MatchStatus.InProgress &&
            match.Status != MatchStatus.Finished)
        {
            throw new InvalidOperationException(
                "Solo se pueden registrar eventos en partidos en curso o finalizados.");
        }
    }
}