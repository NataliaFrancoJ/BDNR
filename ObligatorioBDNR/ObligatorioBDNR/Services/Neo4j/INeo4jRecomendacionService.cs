using ObligatorioBDNR.Models.Recomendacion;

namespace ObligatorioBDNR.Services.Neo4j;

public interface INeo4jRecomendacionService
{
    // Métodos para cargar datos
    Task CrearUsuarioAsync(Usuario usuario);
    Task CrearIdiomaAsync(Idioma idioma);
    Task CrearUnidadAsync(Unidad unidad);
    Task CrearHabilidadAsync(Habilidad habilidad);
    Task CrearEjercicioAsync(Ejercicio ejercicio);
    Task CrearRelacionEstudiaAsync(RelacionEstudia relacion);
    Task CrearRelacionDelIdiomaUnidadAsync(string unidadId, string idiomaId);
    Task CrearRelacionDelIdiomaHabilidadAsync(string habilidadId, string idiomaId);
    Task CrearRelacionPerteneceAAsync(RelacionPerteneceA relacion);
    Task CrearRelacionRefuerzaAsync(string ejercicioId, string habilidadId);
    Task CrearRelacionRealizaAsync(RelacionRealiza relacion);
    Task CrearRelacionFallaEnAsync(RelacionFallaEn relacion);
    Task CrearRelacionSimilarAAsync(RelacionSimilarA relacion);
    Task CrearRelacionPrecedeAAsync(RelacionPrecedeA relacion);

    // Métodos para consultar recomendaciones (5 patrones)
    Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorDificultadesAsync(string usuarioId);
    Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorSimilitudUsuariosAsync(string usuarioId);
    Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorContenidoCursoAsync(string usuarioId, string? idiomaId = null);
    Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorIdiomaAsync(string usuarioId, string? idiomaId = null);
    Task<List<RecomendacionResultado>> ObtenerRecomendacionesCombinadasAsync(string usuarioId, string? idiomaId = null);

    // Método para cargar datos de ejemplo
    Task CargarDatosEjemploAsync();

    // Métodos de diagnóstico
    Task<bool> VerificarDatosCargadosAsync();
    Task<string> ObtenerDiagnosticoAsync(string usuarioId);
}

