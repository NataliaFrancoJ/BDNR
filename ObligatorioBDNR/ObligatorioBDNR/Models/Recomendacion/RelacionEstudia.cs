namespace ObligatorioBDNR.Models.Recomendacion;

public class RelacionEstudia
{
    public string UsuarioId { get; set; } = string.Empty;
    public string IdiomaId { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaUltimaActividad { get; set; }
}

