using System.ComponentModel.DataAnnotations;
using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request;

public class PlayerRequestDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime BirthDate { get; set; }

    [Range(1, 99, ErrorMessage = "El número de camiseta debe estar entre 1 y 99.")]
    public int Number { get; set; }

    [Required(ErrorMessage = "La posición es obligatoria.")]
    public PlayerPosition Position { get; set; }

    [Required(ErrorMessage = "El equipo es obligatorio.")]
    public int TeamId { get; set; }
}
