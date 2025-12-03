namespace ObligatorioBDNR.Models.Recomendacion;

public class RecomendacionResultado
{
    public string IdEjercicio { get; set; } = string.Empty;
    public string NombreEjercicio { get; set; } = string.Empty;
    public string? Habilidad { get; set; }
    public int? VecesFalladas { get; set; }
    public string? Unidad { get; set; }
    public string? Idioma { get; set; }
    public string? TipoRecomendacion { get; set; }
    public double? Score { get; set; }
}

