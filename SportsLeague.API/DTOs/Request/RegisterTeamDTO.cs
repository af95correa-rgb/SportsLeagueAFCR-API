using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request;

public class RegisterTeamDTO
{
    [Required(ErrorMessage = "El ID del equipo es obligatorio.")]
    public int TeamId { get; set; }
}
