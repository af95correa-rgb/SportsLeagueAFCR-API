using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request;

public class TeamRequestDTO
{
    [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "El estadio es obligatorio.")]
    public string Stadium { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    [Required(ErrorMessage = "La fecha de fundación es obligatoria.")]
    public DateTime FoundedDate { get; set; }
}
