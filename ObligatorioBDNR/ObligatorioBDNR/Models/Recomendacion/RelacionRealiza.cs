namespace ObligatorioBDNR.Models.Recomendacion;

public class RelacionRealiza
{
    public string UsuarioId { get; set; } = string.Empty;
    public string EjercicioId { get; set; } = string.Empty;
    public DateTime FechaRealizado { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public int Tiempo { get; set; }
    public int Intentos { get; set; }
}

