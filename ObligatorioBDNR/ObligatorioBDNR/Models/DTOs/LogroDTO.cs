using System.ComponentModel.DataAnnotations;

namespace ObligatorioBDNR.Models.DTOs;

/// <summary>
/// DTO para crear/editar logros
/// </summary>
public class LogroDTO
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del logro es requerido")]
    [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "La recompensa de XP debe ser mayor o igual a 0")]
    public int XpRecompensa { get; set; } = 0;
}

