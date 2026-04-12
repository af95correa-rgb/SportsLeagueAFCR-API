using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request;

public class RefereeRequestDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    public string Nationality { get; set; } = string.Empty;
}
