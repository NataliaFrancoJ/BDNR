using System.ComponentModel.DataAnnotations;

namespace ObligatorioBDNR.Models.DTOs;

/// <summary>
/// DTO para crear/editar usuarios
/// </summary>
public class UsuarioDTO
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [MinLength(3, ErrorMessage = "El nombre de usuario debe tener al menos 3 caracteres")]
    public string Username { get; set; } = string.Empty;

    public string FotoPerfil { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    public bool Tiene2FA { get; set; }
    public string Metodo2FA { get; set; } = string.Empty;

    // Configuración de privacidad
    public bool PerfilVisible { get; set; } = true;
    public bool MostrarRacha { get; set; } = true;
    public bool PermitirMensajes { get; set; } = true;
    public string CompartirActividad { get; set; } = "Amigos";

    // Progreso
    public string IdiomaPrincipal { get; set; } = "Español";
    public int NivelesCompletados { get; set; } = 0;
    public int XpTotal { get; set; } = 0;

    // Suscripción
    public string TipoSuscripcion { get; set; } = "Gratis";
    public bool AutoRenovacion { get; set; } = false;

    // Preferencias
    public List<string> IdiomasInteres { get; set; } = new();
    public bool NotificacionesEmail { get; set; } = true;
    public bool NotificacionesPush { get; set; } = true;
    public bool RecordatoriosDiarios { get; set; } = true;
}

