using System.ComponentModel.DataAnnotations;
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request;

public class UpdateStatusDTO
{
    [Required(ErrorMessage = "El estado es obligatorio.")]
    public TournamentStatus Status { get; set; }
}
