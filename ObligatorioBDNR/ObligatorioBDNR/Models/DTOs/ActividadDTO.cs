using System.ComponentModel.DataAnnotations;

namespace ObligatorioBDNR.Models.DTOs;

/// <summary>
/// DTO para registrar actividad
/// </summary>
public class ActividadDTO
{
    [Required(ErrorMessage = "El ID de usuario es requerido")]
    public Guid IdUsuario { get; set; }

    [Required(ErrorMessage = "La fecha es requerida")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El tipo de acción es requerido")]
    public string TipoAccion { get; set; } = string.Empty;

    public string IdLeccion { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "El XP ganado debe ser mayor o igual a 0")]
    public int XpGanado { get; set; } = 0;
}

