using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request;

public class TournamentRequestDTO
{
    [Required(ErrorMessage = "El nombre del torneo es obligatorio.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La temporada es obligatoria.")]
    public string Season { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime EndDate { get; set; }
}
