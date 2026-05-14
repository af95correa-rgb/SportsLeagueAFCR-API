using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Team
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        // Player
        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty));

        // Referee
        CreateMap<RefereeRequestDTO, Referee>();
        CreateMap<Referee, RefereeResponseDTO>();

        // Tournament
        CreateMap<TournamentRequestDTO, Tournament>();
        CreateMap<Tournament, TournamentResponseDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TeamsCount, opt => opt.MapFrom(src => src.TournamentTeams.Count));

        // Sponsor
        CreateMap<Sponsor, SponsorResponseDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
        CreateMap<SponsorRequestDTO, Sponsor>();

        // Match ← NUEVOS
        CreateMap<MatchRequestDTO, Match>();
        CreateMap<Match, MatchResponseDTO>()
            .ForMember(dest => dest.TournamentName,
                opt => opt.MapFrom(src => src.Tournament.Name))
            .ForMember(dest => dest.HomeTeamName,
                opt => opt.MapFrom(src => src.HomeTeam.Name))
            .ForMember(dest => dest.AwayTeamName,
                opt => opt.MapFrom(src => src.AwayTeam.Name))
            .ForMember(dest => dest.RefereeFullName,
                opt => opt.MapFrom(src => src.Referee.FirstName + " " + src.Referee.LastName));
    }
}