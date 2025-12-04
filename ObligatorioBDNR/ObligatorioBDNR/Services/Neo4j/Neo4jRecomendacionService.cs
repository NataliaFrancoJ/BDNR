using Neo4j.Driver;
using ObligatorioBDNR.Data.Neo4j;
using ObligatorioBDNR.Models.Recomendacion;
using System.Linq;

namespace ObligatorioBDNR.Services.Neo4j;

public class Neo4jRecomendacionService : INeo4jRecomendacionService
{
    private readonly Neo4jContext _context;

    public Neo4jRecomendacionService(Neo4jContext context)
    {
        _context = context;
    }

    #region Métodos para cargar datos

    public async Task CrearUsuarioAsync(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.IdUsuario))
        {
            throw new Exception("El ID de usuario no puede estar vacío.");
        }

        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync(
            "MATCH (u:Usuario {idUsuario: $idUsuario}) RETURN u.idUsuario AS id",
            new { idUsuario = usuario.IdUsuario }
        );
        
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = true;
            break;
        }
        
        if (exists)
        {
            throw new Exception($"Ya existe un usuario con el ID '{usuario.IdUsuario}'. Por favor, usa un ID diferente.");
        }
        
        var createResult = await session.RunAsync(
            "CREATE (u:Usuario {idUsuario: $idUsuario, username: $username}) RETURN u.idUsuario AS id",
            new { idUsuario = usuario.IdUsuario, username = usuario.Username }
        );
        await createResult.ConsumeAsync();
    }

    public async Task CrearIdiomaAsync(Idioma idioma)
    {
        if (string.IsNullOrWhiteSpace(idioma.IdIdioma))
        {
            throw new Exception("El ID de idioma no puede estar vacío.");
        }

        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync(
            "MATCH (i:Idioma {idIdioma: $idIdioma}) RETURN i.idIdioma AS id",
            new { idIdioma = idioma.IdIdioma }
        );
        
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = true;
            break;
        }
        
        if (exists)
        {
            throw new Exception($"Ya existe un idioma con el ID '{idioma.IdIdioma}'. Por favor, usa un ID diferente.");
        }
        
        var createResult = await session.RunAsync(
            "CREATE (i:Idioma {idIdioma: $idIdioma, nombre: $nombre}) RETURN i.idIdioma AS id",
            new { idIdioma = idioma.IdIdioma, nombre = idioma.Nombre }
        );
        await createResult.ConsumeAsync();
    }

    public async Task CrearUnidadAsync(Unidad unidad)
    {
        if (string.IsNullOrWhiteSpace(unidad.IdUnidad))
        {
            throw new Exception("El ID de unidad no puede estar vacío.");
        }

        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync(
            "MATCH (u:Unidad {idUnidad: $idUnidad}) RETURN u.idUnidad AS id",
            new { idUnidad = unidad.IdUnidad }
        );
        
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = true;
            break;
        }
        
        if (exists)
        {
            throw new Exception($"Ya existe una unidad con el ID '{unidad.IdUnidad}'. Por favor, usa un ID diferente.");
        }
        
        var createResult = await session.RunAsync(
            "CREATE (u:Unidad {idUnidad: $idUnidad, nombre: $nombre, posicion: $posicion, descripcion: $descripcion, nivel: $nivel}) RETURN u.idUnidad AS id",
            new
            {
                idUnidad = unidad.IdUnidad,
                nombre = unidad.Nombre,
                posicion = unidad.Posicion,
                descripcion = unidad.Descripcion,
                nivel = unidad.Nivel
            }
        );
        await createResult.ConsumeAsync();
    }

    public async Task CrearHabilidadAsync(Habilidad habilidad)
    {
        if (string.IsNullOrWhiteSpace(habilidad.IdHabilidad))
        {
            throw new Exception("El ID de habilidad no puede estar vacío.");
        }

        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync(
            "MATCH (h:Habilidad {idHabilidad: $idHabilidad}) RETURN h.idHabilidad AS id",
            new { idHabilidad = habilidad.IdHabilidad }
        );
        
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = true;
            break;
        }
        
        if (exists)
        {
            throw new Exception($"Ya existe una habilidad con el ID '{habilidad.IdHabilidad}'. Por favor, usa un ID diferente.");
        }
        
        var createResult = await session.RunAsync(
            "CREATE (h:Habilidad {idHabilidad: $idHabilidad, nombre: $nombre, descripcion: $descripcion, categoria: $categoria}) RETURN h.idHabilidad AS id",
            new
            {
                idHabilidad = habilidad.IdHabilidad,
                nombre = habilidad.Nombre,
                descripcion = habilidad.Descripcion,
                categoria = habilidad.Categoria
            }
        );
        await createResult.ConsumeAsync();
    }

    public async Task CrearEjercicioAsync(Ejercicio ejercicio)
    {
        if (string.IsNullOrWhiteSpace(ejercicio.IdEjercicio))
        {
            throw new Exception("El ID de ejercicio no puede estar vacío.");
        }

        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync(
            "MATCH (e:Ejercicio {idEjercicio: $idEjercicio}) RETURN e.idEjercicio AS id",
            new { idEjercicio = ejercicio.IdEjercicio }
        );
        
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = true;
            break;
        }
        
        if (exists)
        {
            throw new Exception($"Ya existe un ejercicio con el ID '{ejercicio.IdEjercicio}'. Por favor, usa un ID diferente.");
        }
        
        var createResult = await session.RunAsync(
            "CREATE (e:Ejercicio {idEjercicio: $idEjercicio, nombre: $nombre, descripcion: $descripcion, categoria: $categoria}) RETURN e.idEjercicio AS id",
            new
            {
                idEjercicio = ejercicio.IdEjercicio,
                nombre = ejercicio.Nombre,
                descripcion = ejercicio.Descripcion,
                categoria = ejercicio.Categoria
            }
        );
        await createResult.ConsumeAsync();
    }

    public async Task CrearRelacionEstudiaAsync(RelacionEstudia relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.UsuarioId))
        {
            throw new Exception("El ID de usuario no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.IdiomaId))
        {
            throw new Exception("El ID de idioma no puede estar vacío.");
        }

        if (!await ExisteUsuarioAsync(relacion.UsuarioId))
        {
            throw new Exception($"No existe un usuario con el ID '{relacion.UsuarioId}'. Por favor, crea el usuario primero.");
        }
        if (!await ExisteIdiomaAsync(relacion.IdiomaId))
        {
            throw new Exception($"No existe un idioma con el ID '{relacion.IdiomaId}'. Por favor, crea el idioma primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (u)-[:ESTUDIA {nivel: $nivel, fechaInicio: $fechaInicio, fechaUltimaActividad: $fechaUltimaActividad}]->(i)
              RETURN u.idUsuario AS usuarioId, i.idIdioma AS idiomaId",
            new
            {
                usuarioId = relacion.UsuarioId,
                idiomaId = relacion.IdiomaId,
                nivel = relacion.Nivel,
                fechaInicio = relacion.FechaInicio.ToString("yyyy-MM-dd"),
                fechaUltimaActividad = relacion.FechaUltimaActividad.ToString("yyyy-MM-dd")
            }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionDelIdiomaUnidadAsync(string unidadId, string idiomaId)
    {
        if (string.IsNullOrWhiteSpace(unidadId))
        {
            throw new Exception("El ID de unidad no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(idiomaId))
        {
            throw new Exception("El ID de idioma no puede estar vacío.");
        }

        if (!await ExisteUnidadAsync(unidadId))
        {
            throw new Exception($"No existe una unidad con el ID '{unidadId}'. Por favor, crea la unidad primero.");
        }
        if (!await ExisteIdiomaAsync(idiomaId))
        {
            throw new Exception($"No existe un idioma con el ID '{idiomaId}'. Por favor, crea el idioma primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u:Unidad {idUnidad: $unidadId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (u)-[:DEL_IDIOMA]->(i)
              RETURN u.idUnidad AS unidadId, i.idIdioma AS idiomaId",
            new { unidadId, idiomaId }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionDelIdiomaHabilidadAsync(string habilidadId, string idiomaId)
    {
        if (string.IsNullOrWhiteSpace(habilidadId))
        {
            throw new Exception("El ID de habilidad no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(idiomaId))
        {
            throw new Exception("El ID de idioma no puede estar vacío.");
        }

        if (!await ExisteHabilidadAsync(habilidadId))
        {
            throw new Exception($"No existe una habilidad con el ID '{habilidadId}'. Por favor, crea la habilidad primero.");
        }
        if (!await ExisteIdiomaAsync(idiomaId))
        {
            throw new Exception($"No existe un idioma con el ID '{idiomaId}'. Por favor, crea el idioma primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (h:Habilidad {idHabilidad: $habilidadId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (h)-[:DEL_IDIOMA]->(i)
              RETURN h.idHabilidad AS habilidadId, i.idIdioma AS idiomaId",
            new { habilidadId, idiomaId }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionPerteneceAAsync(RelacionPerteneceA relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.EjercicioId))
        {
            throw new Exception("El ID de ejercicio no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.UnidadId))
        {
            throw new Exception("El ID de unidad no puede estar vacío.");
        }

        if (!await ExisteEjercicioAsync(relacion.EjercicioId))
        {
            throw new Exception($"No existe un ejercicio con el ID '{relacion.EjercicioId}'. Por favor, crea el ejercicio primero.");
        }
        if (!await ExisteUnidadAsync(relacion.UnidadId))
        {
            throw new Exception($"No existe una unidad con el ID '{relacion.UnidadId}'. Por favor, crea la unidad primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (u:Unidad {idUnidad: $unidadId})
              CREATE (e)-[:PERTENECE_A {posicion: $posicion}]->(u)
              RETURN e.idEjercicio AS ejercicioId, u.idUnidad AS unidadId",
            new
            {
                ejercicioId = relacion.EjercicioId,
                unidadId = relacion.UnidadId,
                posicion = relacion.Posicion
            }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionRefuerzaAsync(string ejercicioId, string habilidadId)
    {
        if (string.IsNullOrWhiteSpace(ejercicioId))
        {
            throw new Exception("El ID de ejercicio no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(habilidadId))
        {
            throw new Exception("El ID de habilidad no puede estar vacío.");
        }

        if (!await ExisteEjercicioAsync(ejercicioId))
        {
            throw new Exception($"No existe un ejercicio con el ID '{ejercicioId}'. Por favor, crea el ejercicio primero.");
        }
        if (!await ExisteHabilidadAsync(habilidadId))
        {
            throw new Exception($"No existe una habilidad con el ID '{habilidadId}'. Por favor, crea la habilidad primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (h:Habilidad {idHabilidad: $habilidadId})
              CREATE (e)-[:REFUERZA]->(h)
              RETURN e.idEjercicio AS ejercicioId, h.idHabilidad AS habilidadId",
            new { ejercicioId, habilidadId }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionRealizaAsync(RelacionRealiza relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.UsuarioId))
        {
            throw new Exception("El ID de usuario no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.EjercicioId))
        {
            throw new Exception("El ID de ejercicio no puede estar vacío.");
        }

        if (!await ExisteUsuarioAsync(relacion.UsuarioId))
        {
            throw new Exception($"No existe un usuario con el ID '{relacion.UsuarioId}'. Por favor, crea el usuario primero.");
        }
        if (!await ExisteEjercicioAsync(relacion.EjercicioId))
        {
            throw new Exception($"No existe un ejercicio con el ID '{relacion.EjercicioId}'. Por favor, crea el ejercicio primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (e:Ejercicio {idEjercicio: $ejercicioId})
              CREATE (u)-[:REALIZA {fechaRealizado: $fechaRealizado, resultado: $resultado, tiempo: $tiempo, intentos: $intentos}]->(e)
              RETURN u.idUsuario AS usuarioId, e.idEjercicio AS ejercicioId",
            new
            {
                usuarioId = relacion.UsuarioId,
                ejercicioId = relacion.EjercicioId,
                fechaRealizado = relacion.FechaRealizado.ToString("yyyy-MM-ddTHH:mm:ss"),
                resultado = relacion.Resultado,
                tiempo = relacion.Tiempo,
                intentos = relacion.Intentos
            }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionFallaEnAsync(RelacionFallaEn relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.UsuarioId))
        {
            throw new Exception("El ID de usuario no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.HabilidadId))
        {
            throw new Exception("El ID de habilidad no puede estar vacío.");
        }

        if (!await ExisteUsuarioAsync(relacion.UsuarioId))
        {
            throw new Exception($"No existe un usuario con el ID '{relacion.UsuarioId}'. Por favor, crea el usuario primero.");
        }
        if (!await ExisteHabilidadAsync(relacion.HabilidadId))
        {
            throw new Exception($"No existe una habilidad con el ID '{relacion.HabilidadId}'. Por favor, crea la habilidad primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (h:Habilidad {idHabilidad: $habilidadId})
              CREATE (u)-[:FALLA_EN {vecesFalladas: $vecesFalladas}]->(h)
              RETURN u.idUsuario AS usuarioId, h.idHabilidad AS habilidadId",
            new
            {
                usuarioId = relacion.UsuarioId,
                habilidadId = relacion.HabilidadId,
                vecesFalladas = relacion.VecesFalladas
            }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionSimilarAAsync(RelacionSimilarA relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.UsuarioId1))
        {
            throw new Exception("El ID de usuario 1 no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.UsuarioId2))
        {
            throw new Exception("El ID de usuario 2 no puede estar vacío.");
        }

        if (!await ExisteUsuarioAsync(relacion.UsuarioId1))
        {
            throw new Exception($"No existe un usuario con el ID '{relacion.UsuarioId1}'. Por favor, crea el usuario primero.");
        }
        if (!await ExisteUsuarioAsync(relacion.UsuarioId2))
        {
            throw new Exception($"No existe un usuario con el ID '{relacion.UsuarioId2}'. Por favor, crea el usuario primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u1:Usuario {idUsuario: $usuarioId1}), (u2:Usuario {idUsuario: $usuarioId2})
              CREATE (u1)-[:SIMILAR_A]->(u2)
              RETURN u1.idUsuario AS usuarioId1, u2.idUsuario AS usuarioId2",
            new { usuarioId1 = relacion.UsuarioId1, usuarioId2 = relacion.UsuarioId2 }
        );
        await result.ConsumeAsync();
    }

    public async Task CrearRelacionPrecedeAAsync(RelacionPrecedeA relacion)
    {
        if (string.IsNullOrWhiteSpace(relacion.UnidadId1))
        {
            throw new Exception("El ID de unidad 1 no puede estar vacío.");
        }
        if (string.IsNullOrWhiteSpace(relacion.UnidadId2))
        {
            throw new Exception("El ID de unidad 2 no puede estar vacío.");
        }

        if (!await ExisteUnidadAsync(relacion.UnidadId1))
        {
            throw new Exception($"No existe una unidad con el ID '{relacion.UnidadId1}'. Por favor, crea la unidad primero.");
        }
        if (!await ExisteUnidadAsync(relacion.UnidadId2))
        {
            throw new Exception($"No existe una unidad con el ID '{relacion.UnidadId2}'. Por favor, crea la unidad primero.");
        }

        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            @"MATCH (u1:Unidad {idUnidad: $unidadId1}), (u2:Unidad {idUnidad: $unidadId2})
              CREATE (u1)-[:PRECEDE_A]->(u2)
              RETURN u1.idUnidad AS unidadId1, u2.idUnidad AS unidadId2",
            new { unidadId1 = relacion.UnidadId1, unidadId2 = relacion.UnidadId2 }
        );
        await result.ConsumeAsync();
    }

    #endregion

    #region Métodos auxiliares para validación

    private async Task<bool> ExisteUsuarioAsync(string usuarioId)
    {
        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            "MATCH (u:Usuario {idUsuario: $usuarioId}) RETURN u.idUsuario AS id",
            new { usuarioId }
        );
        
        await foreach (var record in result)
        {
            return true;
        }
        return false;
    }

    private async Task<bool> ExisteIdiomaAsync(string idiomaId)
    {
        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            "MATCH (i:Idioma {idIdioma: $idiomaId}) RETURN i.idIdioma AS id",
            new { idiomaId }
        );
        
        await foreach (var record in result)
        {
            return true;
        }
        return false;
    }

    private async Task<bool> ExisteUnidadAsync(string unidadId)
    {
        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            "MATCH (u:Unidad {idUnidad: $unidadId}) RETURN u.idUnidad AS id",
            new { unidadId }
        );
        
        await foreach (var record in result)
        {
            return true;
        }
        return false;
    }

    private async Task<bool> ExisteHabilidadAsync(string habilidadId)
    {
        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            "MATCH (h:Habilidad {idHabilidad: $habilidadId}) RETURN h.idHabilidad AS id",
            new { habilidadId }
        );
        
        await foreach (var record in result)
        {
            return true;
        }
        return false;
    }

    private async Task<bool> ExisteEjercicioAsync(string ejercicioId)
    {
        await using var session = _context.GetSession();
        var result = await session.RunAsync(
            "MATCH (e:Ejercicio {idEjercicio: $ejercicioId}) RETURN e.idEjercicio AS id",
            new { ejercicioId }
        );
        
        await foreach (var record in result)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Métodos auxiliares para GDS

    private async Task CrearProyeccionUsuariosEjerciciosAsync()
    {
        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync("CALL gds.graph.exists('usuarios-ejercicios') YIELD exists RETURN exists");
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = record["exists"].As<bool>();
        }

        if (exists)
        {
            return;
        }

        try
        {
            await session.RunAsync("CALL gds.graph.drop('usuarios-ejercicios', false)");
        }
        catch { }

        try
        {
            await session.RunAsync(@"
                CALL gds.graph.project(
                    'usuarios-ejercicios',
                    ['Usuario', 'Ejercicio'],
                    {
                        REALIZA: {
                            type: 'REALIZA',
                            orientation: 'UNDIRECTED'
                        }
                    }
                )");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear proyección GDS 'usuarios-ejercicios': {ex.Message}");
        }
    }

    private async Task CrearProyeccionEjerciciosHabilidadesAsync()
    {
        await using var session = _context.GetSession();
        
        var checkResult = await session.RunAsync("CALL gds.graph.exists('ejercicios-habilidades') YIELD exists RETURN exists");
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = record["exists"].As<bool>();
        }

        if (exists)
        {
            return; 
        }

        try
        {
            await session.RunAsync("CALL gds.graph.drop('ejercicios-habilidades', false)");
        }
        catch { }

        try
        {
            await session.RunAsync(@"
                CALL gds.graph.project(
                    'ejercicios-habilidades',
                    ['Ejercicio', 'Habilidad', 'Unidad', 'Idioma'],
                    {
                        REFUERZA: {
                            type: 'REFUERZA',
                            orientation: 'UNDIRECTED'
                        },
                        PERTENECE_A: {
                            type: 'PERTENECE_A',
                            orientation: 'UNDIRECTED'
                        },
                        DEL_IDIOMA: {
                            type: 'DEL_IDIOMA',
                            orientation: 'UNDIRECTED'
                        }
                    }
                )");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear proyección GDS 'ejercicios-habilidades': {ex.Message}");
        }
    }

    private async Task CrearProyeccionUsuariosHabilidadesAsync()
    {
        await using var session = _context.GetSession();
        
        try
        {
            var checkResult = await session.RunAsync("CALL gds.graph.exists('usuarios-habilidades') YIELD exists RETURN exists");
            var exists = false;
            await foreach (var record in checkResult)
            {
                exists = record["exists"].As<bool>();
            }

            if (exists)
            {
                return;
            }
        }
        catch
        {
        }

        try
        {
            await session.RunAsync("CALL gds.graph.drop('usuarios-habilidades', false)");
        }
        catch { }

        try
        {
            await session.RunAsync(@"
                CALL gds.graph.project(
                    'usuarios-habilidades',
                    ['Usuario', 'Habilidad'],
                    {
                        FALLA_EN: {
                            type: 'FALLA_EN',
                            orientation: 'UNDIRECTED'
                        }
                    }
                )");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Advertencia: No se pudo crear proyección 'usuarios-habilidades': {ex.Message}");
        }
    }

    #endregion

    #region Métodos para consultar recomendaciones
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorDificultadesAsync(string usuarioId)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.GetSession();
        
        var cursor = await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
              MATCH (u)-[f:FALLA_EN]->(h:Habilidad)
              MATCH (h)-[:DEL_IDIOMA]->(idI)
              MATCH (e:Ejercicio)-[:REFUERZA]->(h)
              MATCH (e)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
              RETURN DISTINCT e.idEjercicio AS ejercicio,
                     e.nombre AS nombreEjercicio,
                     h.nombre AS habilidad,
                     f.vecesFalladas AS vecesFalladas,
                     un.nombre AS unidad,
                     idI.nombre AS idioma,
                     (f.vecesFalladas * 2.0) AS score
              ORDER BY f.vecesFalladas DESC, e.idEjercicio
              LIMIT 20",
            new { usuarioId }
        );

        var records = await cursor.ToListAsync();

        foreach (var record in records)
        {
            resultados.Add(new RecomendacionResultado
            {
                IdEjercicio = record["ejercicio"].As<string>() ?? string.Empty,
                NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                Habilidad = record["habilidad"].As<string>(),
                VecesFalladas = record["vecesFalladas"].As<int?>(),
                Unidad = record["unidad"].As<string>(),
                Idioma = record["idioma"].As<string>(),
                Score = record["score"].As<double?>(),
                TipoRecomendacion = "Basada en dificultades del usuario"
            });
        }

        return resultados;
    }

    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorSimilitudUsuariosAsync(string usuarioId)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.GetSession();
        
        await CrearProyeccionUsuariosEjerciciosAsync();

        var similarityResult = await session.RunAsync(@"
            CALL gds.nodeSimilarity.stream('usuarios-ejercicios', {
                similarityCutoff: 0.1,
                topK: 10
            })
            YIELD node1, node2, similarity
            MATCH (u1:Usuario) WHERE id(u1) = node1
            MATCH (u2:Usuario) WHERE id(u2) = node2
            MATCH (u1 {idUsuario: $usuarioId})
            RETURN u2.idUsuario AS usuarioSimilar, similarity
            ORDER BY similarity DESC
            LIMIT 5",
            new { usuarioId }
        );

        var usuariosSimilares = new List<(string UsuarioId, double Similarity)>();
        await foreach (var record in similarityResult)
        {
            usuariosSimilares.Add((record["usuarioSimilar"].As<string>(), record["similarity"].As<double>()));
        }

        if (!usuariosSimilares.Any())
        {
            var fallbackResult = await session.RunAsync(
                @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                  MATCH (u)-[:SIMILAR_A]->(u2:Usuario)
                  MATCH (u2)-[:REALIZA]->(e:Ejercicio)
                  MATCH (e)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                  WHERE NOT (u)-[:REALIZA]->(e)
                  RETURN DISTINCT e.idEjercicio AS ejercicio,
                         e.nombre AS nombreEjercicio,
                         un.nombre AS unidad,
                         idI.nombre AS idioma,
                         count(*) AS score
                  ORDER BY score DESC
                  LIMIT 20",
                new { usuarioId }
            );

            await foreach (var record in fallbackResult)
            {
                resultados.Add(new RecomendacionResultado
                {
                    IdEjercicio = record["ejercicio"].As<string>() ?? string.Empty,
                    NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                    Unidad = record["unidad"].As<string>(),
                    Idioma = record["idioma"].As<string>(),
                    Score = record["score"].As<double?>(),
                    TipoRecomendacion = "Basada en similitud de usuarios (GDS - Fallback)"
                });
            }
            return resultados;
        }

        var usuarioIds = usuariosSimilares.Select(u => u.UsuarioId).ToList();
        var similarityMap = usuariosSimilares.ToDictionary(u => u.UsuarioId, u => u.Similarity);
        
        var result = await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
              MATCH (u2:Usuario)-[:REALIZA]->(e:Ejercicio)
              MATCH (e)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
              WHERE u2.idUsuario IN $usuarioIds AND NOT (u)-[:REALIZA]->(e)
              RETURN DISTINCT e.idEjercicio AS ejercicio,
                     e.nombre AS nombreEjercicio,
                     un.nombre AS unidad,
                     idI.nombre AS idioma,
                     u2.idUsuario AS usuarioSimilar,
                     count(*) AS score
              ORDER BY score DESC
              LIMIT 20",
            new { usuarioId, usuarioIds }
        );

        var ejerciciosDict = new Dictionary<string, RecomendacionResultado>();
        await foreach (var record in result)
        {
            var ejercicioId = record["ejercicio"].As<string>();
            var usuarioSimilar = record["usuarioSimilar"].As<string>();
            var similarity = similarityMap.ContainsKey(usuarioSimilar) ? similarityMap[usuarioSimilar] : 0.1;
            
            if (!ejerciciosDict.ContainsKey(ejercicioId))
            {
                ejerciciosDict[ejercicioId] = new RecomendacionResultado
                {
                    IdEjercicio = ejercicioId,
                    NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                    Unidad = record["unidad"].As<string>(),
                    Idioma = record["idioma"].As<string>(),
                    Score = 0.0,
                    TipoRecomendacion = "Basada en similitud de usuarios (GDS)"
                };
            }
            ejerciciosDict[ejercicioId].Score = (ejerciciosDict[ejercicioId].Score ?? 0.0) + similarity;
        }
        
        resultados = ejerciciosDict.Values.OrderByDescending(e => e.Score).Take(20).ToList();
        return resultados;
    }

    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorContenidoCursoAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.GetSession();
        
        var ejerciciosScores = new Dictionary<string, double>();
        bool usarGDS = false;

        try
        {
            await CrearProyeccionEjerciciosHabilidadesAsync();

            string graphName = "ejercicios-habilidades";
            
            var pageRankResult = await session.RunAsync(@"
                CALL gds.pageRank.stream($graphName, {
                    maxIterations: 20,
                    dampingFactor: 0.85
                })
                YIELD nodeId, score
                MATCH (e:Ejercicio) WHERE id(e) = nodeId
                RETURN e.idEjercicio AS ejercicio, score
                ORDER BY score DESC
                LIMIT 50",
                new { graphName }
            );

            await foreach (var record in pageRankResult)
            {
                ejerciciosScores[record["ejercicio"].As<string>()] = record["score"].As<double>();
            }

            usarGDS = ejerciciosScores.Any();
        }
        catch
        {
            usarGDS = false;
        }

        if (!usarGDS)
        {
            string queryFallback = idiomaId == null
                ? @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                    MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                    WHERE NOT (u)-[:REALIZA]->(e)
                    RETURN DISTINCT e.idEjercicio AS ejercicio,
                           e.nombre AS nombreEjercicio,
                           h.nombre AS habilidad,
                           un.nombre AS unidad,
                           un.posicion AS posicion,
                           idI.nombre AS idioma,
                           1.0 AS score
                    ORDER BY un.posicion ASC
                    LIMIT 20"
                : @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma {idIdioma: $idiomaId})
                    MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                    WHERE NOT (u)-[:REALIZA]->(e)
                    RETURN DISTINCT e.idEjercicio AS ejercicio,
                           e.nombre AS nombreEjercicio,
                           h.nombre AS habilidad,
                           un.nombre AS unidad,
                           un.posicion AS posicion,
                           idI.nombre AS idioma,
                           1.0 AS score
                    ORDER BY un.posicion ASC
                    LIMIT 20";

            object parametersFallback = idiomaId == null
                ? (object)new { usuarioId }
                : new { usuarioId, idiomaId };

            var resultFallback = await session.RunAsync(queryFallback, parametersFallback);

            await foreach (var record in resultFallback)
            {
                resultados.Add(new RecomendacionResultado
                {
                    IdEjercicio = record["ejercicio"].As<string>(),
                    NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                    Habilidad = record["habilidad"].As<string>(),
                    Unidad = record["unidad"].As<string>(),
                    Idioma = record["idioma"].As<string>(),
                    Score = record["score"].As<double>(),
                    TipoRecomendacion = "Basada en contenido y estructura del curso"
                });
            }

            return resultados;
        }

        string query = idiomaId == null
            ? @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                WHERE NOT (u)-[:REALIZA]->(e) AND e.idEjercicio IN $ejerciciosIds
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma
                ORDER BY un.posicion ASC
                LIMIT 20"
            : @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma {idIdioma: $idiomaId})
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                WHERE NOT (u)-[:REALIZA]->(e) AND e.idEjercicio IN $ejerciciosIds
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma
                ORDER BY un.posicion ASC
                LIMIT 20";

        var ejerciciosIds = ejerciciosScores.Keys.ToList();
        object parameters = idiomaId == null
            ? (object)new { usuarioId, ejerciciosIds }
            : new { usuarioId, idiomaId, ejerciciosIds };

        var result = await session.RunAsync(query, parameters);

        await foreach (var record in result)
        {
            var ejercicioId = record["ejercicio"].As<string>();
            var pageRankScore = ejerciciosScores.ContainsKey(ejercicioId) ? ejerciciosScores[ejercicioId] : 0.0;
            
            resultados.Add(new RecomendacionResultado
            {
                IdEjercicio = ejercicioId,
                NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                Habilidad = record["habilidad"].As<string>(),
                Unidad = record["unidad"].As<string>(),
                Idioma = record["idioma"].As<string>(),
                Score = pageRankScore,
                TipoRecomendacion = "Basada en contenido y estructura del curso (GDS PageRank)"
            });
        }

        resultados = resultados.OrderByDescending(r => r.Score).ThenBy(r => 
        {
            var posicionStr = r.Unidad?.Split(' ').LastOrDefault() ?? "0";
            return int.TryParse(posicionStr, out var pos) ? pos : 999;
        }).Take(20).ToList();

        return resultados;
    }

    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorIdiomaAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.GetSession();
        
        var ejerciciosScores = new Dictionary<string, double>();
        bool usarGDS = false;

        try
        {
            await CrearProyeccionEjerciciosHabilidadesAsync();

            var centralityResult = await session.RunAsync(@"
                CALL gds.betweenness.stream('ejercicios-habilidades')
                YIELD nodeId, score
                MATCH (e:Ejercicio) WHERE id(e) = nodeId
                RETURN e.idEjercicio AS ejercicio, score
                ORDER BY score DESC
                LIMIT 50",
                new { }
            );

            await foreach (var record in centralityResult)
            {
                ejerciciosScores[record["ejercicio"].As<string>()] = record["score"].As<double>();
            }

            usarGDS = ejerciciosScores.Any();
        }
        catch
        {
            usarGDS = false;
        }

        if (!usarGDS)
        {
            string queryFallback = idiomaId == null
                ? @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                    MATCH (un:Unidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (h:Habilidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (e:Ejercicio)-[:PERTENECE_A]->(un)
                    MATCH (e)-[:REFUERZA]->(h)
                    WHERE NOT (u)-[:REALIZA]->(e)
                    RETURN DISTINCT e.idEjercicio AS ejercicio,
                           e.nombre AS nombreEjercicio,
                           h.nombre AS habilidad,
                           un.nombre AS unidad,
                           un.posicion AS posicion,
                           idI.nombre AS idioma,
                           1.0 AS score
                    ORDER BY un.posicion ASC
                    LIMIT 20"
                : @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma {idIdioma: $idiomaId})
                    MATCH (un:Unidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (h:Habilidad)-[:DEL_IDIOMA]->(idI)
                    MATCH (e:Ejercicio)-[:PERTENECE_A]->(un)
                    MATCH (e)-[:REFUERZA]->(h)
                    WHERE NOT (u)-[:REALIZA]->(e)
                    RETURN DISTINCT e.idEjercicio AS ejercicio,
                           e.nombre AS nombreEjercicio,
                           h.nombre AS habilidad,
                           un.nombre AS unidad,
                           un.posicion AS posicion,
                           idI.nombre AS idioma,
                           1.0 AS score
                    ORDER BY un.posicion ASC
                    LIMIT 20";

            object parametersFallback = idiomaId == null
                ? (object)new { usuarioId }
                : new { usuarioId, idiomaId };

            var resultFallback = await session.RunAsync(queryFallback, parametersFallback);

            await foreach (var record in resultFallback)
            {
                resultados.Add(new RecomendacionResultado
                {
                    IdEjercicio = record["ejercicio"].As<string>(),
                    NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                    Habilidad = record["habilidad"].As<string>(),
                    Unidad = record["unidad"].As<string>(),
                    Idioma = record["idioma"].As<string>(),
                    Score = record["score"].As<double>(),
                    TipoRecomendacion = "Basada en idioma estudiado"
                });
            }

            return resultados;
        }

        string query = idiomaId == null
            ? @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                MATCH (un:Unidad)-[:DEL_IDIOMA]->(idI)
                MATCH (h:Habilidad)-[:DEL_IDIOMA]->(idI)
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un)
                MATCH (e)-[:REFUERZA]->(h)
                WHERE NOT (u)-[:REALIZA]->(e) AND e.idEjercicio IN $ejerciciosIds
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma
                ORDER BY un.posicion ASC
                LIMIT 20"
            : @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma {idIdioma: $idiomaId})
                MATCH (un:Unidad)-[:DEL_IDIOMA]->(idI)
                MATCH (h:Habilidad)-[:DEL_IDIOMA]->(idI)
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un)
                MATCH (e)-[:REFUERZA]->(h)
                WHERE NOT (u)-[:REALIZA]->(e) AND e.idEjercicio IN $ejerciciosIds
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma
                ORDER BY un.posicion ASC
                LIMIT 20";

        var ejerciciosIds = ejerciciosScores.Keys.ToList();
        object parameters = idiomaId == null
            ? (object)new { usuarioId, ejerciciosIds }
            : new { usuarioId, idiomaId, ejerciciosIds };

        var result = await session.RunAsync(query, parameters);

        await foreach (var record in result)
        {
            var ejercicioId = record["ejercicio"].As<string>();
            var centralityScore = ejerciciosScores.ContainsKey(ejercicioId) ? ejerciciosScores[ejercicioId] : 0.0;
            
            resultados.Add(new RecomendacionResultado
            {
                IdEjercicio = ejercicioId,
                NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                Habilidad = record["habilidad"].As<string>(),
                Unidad = record["unidad"].As<string>(),
                Idioma = record["idioma"].As<string>(),
                Score = centralityScore,
                TipoRecomendacion = "Basada en idioma estudiado (GDS Betweenness)"
            });
        }

        resultados = resultados.OrderByDescending(r => r.Score).ThenBy(r => 
        {
            var posicionStr = r.Unidad?.Split(' ').LastOrDefault() ?? "0";
            return int.TryParse(posicionStr, out var pos) ? pos : 999;
        }).Take(20).ToList();

        return resultados;
    }

    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesCombinadasAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.GetSession();
        
        await CrearProyeccionUsuariosEjerciciosAsync();
        await CrearProyeccionEjerciciosHabilidadesAsync();
        await CrearProyeccionUsuariosHabilidadesAsync();

        var similarityResult = await session.RunAsync(@"
            CALL gds.nodeSimilarity.stream('usuarios-ejercicios', {
                similarityCutoff: 0.1,
                topK: 5
            })
            YIELD node1, node2, similarity
            MATCH (u1:Usuario) WHERE id(u1) = node1
            MATCH (u2:Usuario) WHERE id(u2) = node2
            MATCH (u1 {idUsuario: $usuarioId})
            RETURN u2.idUsuario AS usuarioSimilar, similarity
            ORDER BY similarity DESC
            LIMIT 5",
            new { usuarioId }
        );

        var usuariosSimilares = new List<string>();
        await foreach (var record in similarityResult)
        {
            usuariosSimilares.Add(record["usuarioSimilar"].As<string>());
        }

        var pageRankResult = await session.RunAsync(@"
            CALL gds.pageRank.stream('ejercicios-habilidades', {
                maxIterations: 20,
                dampingFactor: 0.85
            })
            YIELD nodeId, score
            MATCH (e:Ejercicio) WHERE id(e) = nodeId
            RETURN e.idEjercicio AS ejercicio, score
            ORDER BY score DESC
            LIMIT 100",
            new { }
        );

        var ejerciciosPageRank = new Dictionary<string, double>();
        await foreach (var record in pageRankResult)
        {
            ejerciciosPageRank[record["ejercicio"].As<string>()] = record["score"].As<double>();
        }

        string query = idiomaId == null
            ? @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                OPTIONAL MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                OPTIONAL MATCH (u)-[f:FALLA_EN]->(h)
                OPTIONAL MATCH (u2:Usuario)-[:REALIZA]->(e)
                WHERE NOT (u)-[:REALIZA]->(e) AND (u2.idUsuario IN $usuariosSimilares OR u2 IS NULL)
                WITH e, h, f, un, idI, u2,
                     CASE WHEN f IS NOT NULL THEN f.vecesFalladas ELSE 0 END AS vecesFalladas,
                     CASE WHEN u2 IS NOT NULL THEN 1 ELSE 0 END AS similarScore
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       vecesFalladas AS vecesFalladas,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma,
                       (vecesFalladas * 2.0 + similarScore * 1.5) AS baseScore
                ORDER BY baseScore DESC, posicion ASC
                LIMIT 50"
            : @"MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma {idIdioma: $idiomaId})
                MATCH (e:Ejercicio)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                OPTIONAL MATCH (e)-[:REFUERZA]->(h:Habilidad)-[:DEL_IDIOMA]->(idI)
                OPTIONAL MATCH (u)-[f:FALLA_EN]->(h)
                OPTIONAL MATCH (u2:Usuario)-[:REALIZA]->(e)
                WHERE NOT (u)-[:REALIZA]->(e) AND (u2.idUsuario IN $usuariosSimilares OR u2 IS NULL)
                WITH e, h, f, un, idI, u2,
                     CASE WHEN f IS NOT NULL THEN f.vecesFalladas ELSE 0 END AS vecesFalladas,
                     CASE WHEN u2 IS NOT NULL THEN 1 ELSE 0 END AS similarScore
                RETURN DISTINCT e.idEjercicio AS ejercicio,
                       e.nombre AS nombreEjercicio,
                       h.nombre AS habilidad,
                       vecesFalladas AS vecesFalladas,
                       un.nombre AS unidad,
                       un.posicion AS posicion,
                       idI.nombre AS idioma,
                       (vecesFalladas * 2.0 + similarScore * 1.5) AS baseScore
                ORDER BY baseScore DESC, posicion ASC
                LIMIT 50";

        object parameters = idiomaId == null
            ? (object)new { usuarioId, usuariosSimilares = usuariosSimilares.Any() ? usuariosSimilares : new List<string> { "" } }
            : new { usuarioId, idiomaId, usuariosSimilares = usuariosSimilares.Any() ? usuariosSimilares : new List<string> { "" } };

        var result = await session.RunAsync(query, parameters);

        var ejerciciosDict = new Dictionary<string, RecomendacionResultado>();
        await foreach (var record in result)
        {
            var ejercicioId = record["ejercicio"].As<string>();
            var baseScore = record["baseScore"].As<double>();
            var pageRankScore = ejerciciosPageRank.ContainsKey(ejercicioId) ? ejerciciosPageRank[ejercicioId] : 0.0;
            
            var combinedScore = baseScore + (pageRankScore * 10.0); 

            if (!ejerciciosDict.ContainsKey(ejercicioId))
            {
                ejerciciosDict[ejercicioId] = new RecomendacionResultado
                {
                    IdEjercicio = ejercicioId,
                    NombreEjercicio = record["nombreEjercicio"].As<string>() ?? string.Empty,
                    Habilidad = record["habilidad"].As<string>(),
                    VecesFalladas = record["vecesFalladas"].As<int?>(),
                    Unidad = record["unidad"].As<string>(),
                    Idioma = record["idioma"].As<string>(),
                    Score = combinedScore,
                    TipoRecomendacion = "Recomendación combinada (GDS Multi-Algorithm)"
                };
            }
        }

        resultados = ejerciciosDict.Values.OrderByDescending(e => e.Score).Take(20).ToList();
        return resultados;
    }

    #endregion

    #region Métodos de diagnóstico

    public async Task<bool> VerificarDatosCargadosAsync()
    {
        try
        {
            await using var session = _context.GetSession();
            
            var cursor = await session.RunAsync(@"
                MATCH (u:Usuario)
                OPTIONAL MATCH (u)-[:ESTUDIA]->(i:Idioma)
                OPTIONAL MATCH (e:Ejercicio)
                RETURN count(DISTINCT u) AS usuarios,
                       count(DISTINCT i) AS idiomas,
                       count(DISTINCT e) AS ejercicios"
            );
            
            var result = await cursor.SingleAsync();

            var usuarios = result["usuarios"].As<int>();
            var idiomas = result["idiomas"].As<int>();
            var ejercicios = result["ejercicios"].As<int>();

            return usuarios > 0 && idiomas > 0 && ejercicios > 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al verificar datos: {ex.Message}");
            return false;
        }
    }

    public async Task<string> ObtenerDiagnosticoAsync(string usuarioId)
    {
        var diagnostico = new System.Text.StringBuilder();
        
        try
        {
            await using var session = _context.GetSession();
            
            var conteo = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(@"
                    MATCH (u:Usuario) WITH count(u) AS usuarios
                    MATCH (i:Idioma) WITH usuarios, count(i) AS idiomas
                    MATCH (e:Ejercicio) WITH usuarios, idiomas, count(e) AS ejercicios
                    MATCH (h:Habilidad) WITH usuarios, idiomas, ejercicios, count(h) AS habilidades
                    RETURN usuarios, idiomas, ejercicios, habilidades"
                );
                return await cursor.SingleAsync();
            });
            
            diagnostico.AppendLine($"✓ Conexión OK");
            diagnostico.AppendLine($"  - Usuarios: {conteo["usuarios"].As<int>()}");
            diagnostico.AppendLine($"  - Idiomas: {conteo["idiomas"].As<int>()}");
            diagnostico.AppendLine($"  - Ejercicios: {conteo["ejercicios"].As<int>()}");
            diagnostico.AppendLine($"  - Habilidades: {conteo["habilidades"].As<int>()}");

            var usuarioExiste = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(
                    "MATCH (u:Usuario {idUsuario: $usuarioId}) RETURN u.username AS username",
                    new { usuarioId }
                );
                return await cursor.ToListAsync();
            });

            if (usuarioExiste.Count == 0)
            {
                diagnostico.AppendLine($"✗ Usuario '{usuarioId}' NO existe");
                return diagnostico.ToString();
            }
            diagnostico.AppendLine($"✓ Usuario '{usuarioId}' existe: {usuarioExiste[0]["username"].As<string>()}");

            var relaciones = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(@"
                    MATCH (u:Usuario {idUsuario: $usuarioId})
                    OPTIONAL MATCH (u)-[:ESTUDIA]->(i:Idioma)
                    OPTIONAL MATCH (u)-[:FALLA_EN]->(h:Habilidad)
                    RETURN collect(DISTINCT i.idIdioma) AS idiomas, 
                           collect(DISTINCT h.idHabilidad) AS habilidades",
                    new { usuarioId }
                );
                return await cursor.SingleAsync();
            });

            var idiomasUsuario = relaciones["idiomas"].As<List<string>>();
            var habilidadesUsuario = relaciones["habilidades"].As<List<string>>();
            
            diagnostico.AppendLine($"  - Idiomas que estudia: {string.Join(", ", idiomasUsuario.Where(x => x != null))}");
            diagnostico.AppendLine($"  - Habilidades donde falla: {string.Join(", ", habilidadesUsuario.Where(x => x != null))}");

            var recomendaciones = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync(@"
                    MATCH (u:Usuario {idUsuario: $usuarioId})-[:ESTUDIA]->(idI:Idioma)
                    MATCH (u)-[f:FALLA_EN]->(h:Habilidad)
                    MATCH (h)-[:DEL_IDIOMA]->(idI)
                    MATCH (e:Ejercicio)-[:REFUERZA]->(h)
                    MATCH (e)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)
                    RETURN count(DISTINCT e) AS total",
                    new { usuarioId }
                );
                return await cursor.SingleAsync();
            });

            var totalRecomendaciones = recomendaciones["total"].As<int>();
            diagnostico.AppendLine($"  - Ejercicios recomendables: {totalRecomendaciones}");

            if (totalRecomendaciones == 0)
            {
                diagnostico.AppendLine("✗ No hay ejercicios que cumplan todos los criterios");
            }
            else
            {
                diagnostico.AppendLine("✓ Hay ejercicios disponibles para recomendar");
            }
        }
        catch (Exception ex)
        {
            diagnostico.AppendLine($"✗ Error: {ex.Message}");
        }

        return diagnostico.ToString();
    }

    #endregion

    #region Método para cargar datos de ejemplo

    public async Task CargarDatosEjemploAsync()
    {
        await using var session = _context.GetSession();

        var checkResult = await session.RunAsync("MATCH (n) RETURN count(n) AS total LIMIT 1");
        var record = await checkResult.SingleAsync();
        var totalNodes = record["total"].As<long>();
        
        if (totalNodes > 0)
        {
            return;
        }

        await session.RunAsync("MATCH (n) DETACH DELETE n");

        var usuarios = new[]
        {
            new Usuario { IdUsuario = "u1", Username = "Laura" },
            new Usuario { IdUsuario = "u2", Username = "Carlos" },
            new Usuario { IdUsuario = "u3", Username = "Ana" },
            new Usuario { IdUsuario = "u4", Username = "Pedro" },
            new Usuario { IdUsuario = "u5", Username = "María" },
            new Usuario { IdUsuario = "u6", Username = "Juan" },
            new Usuario { IdUsuario = "u7", Username = "Sofía" },
            new Usuario { IdUsuario = "u8", Username = "Diego" },
            new Usuario { IdUsuario = "u9", Username = "Elena" },
            new Usuario { IdUsuario = "u10", Username = "Miguel" },
            new Usuario { IdUsuario = "u11", Username = "Carmen" },
            new Usuario { IdUsuario = "u12", Username = "Roberto" },
            new Usuario { IdUsuario = "u13", Username = "Isabel" },
            new Usuario { IdUsuario = "u14", Username = "Fernando" },
            new Usuario { IdUsuario = "u15", Username = "Lucía" },
            new Usuario { IdUsuario = "u16", Username = "Alejandro" },
            new Usuario { IdUsuario = "u17", Username = "Patricia" },
            new Usuario { IdUsuario = "u18", Username = "Javier" },
            new Usuario { IdUsuario = "u19", Username = "Marta" },
            new Usuario { IdUsuario = "u20", Username = "Daniel" },
            new Usuario { IdUsuario = "u21", Username = "Cristina" },
            new Usuario { IdUsuario = "u22", Username = "Pablo" },
            new Usuario { IdUsuario = "u23", Username = "Andrea" },
            new Usuario { IdUsuario = "u24", Username = "Sergio" },
            new Usuario { IdUsuario = "u25", Username = "Natalia" },
            new Usuario { IdUsuario = "u26", Username = "Ricardo" },
            new Usuario { IdUsuario = "u27", Username = "Beatriz" },
            new Usuario { IdUsuario = "u28", Username = "Manuel" },
            new Usuario { IdUsuario = "u29", Username = "Rosa" },
            new Usuario { IdUsuario = "u30", Username = "Antonio" }
        };

        foreach (var usuario in usuarios)
        {
            await session.RunAsync(
                "CREATE (u:Usuario {idUsuario: $idUsuario, username: $username})",
                new { idUsuario = usuario.IdUsuario, username = usuario.Username }
            );
        }

        var idiomas = new[]
        {
            new Idioma { IdIdioma = "en", Nombre = "Inglés" },
            new Idioma { IdIdioma = "es", Nombre = "Español" },
            new Idioma { IdIdioma = "fr", Nombre = "Francés" },
            new Idioma { IdIdioma = "de", Nombre = "Alemán" },
            new Idioma { IdIdioma = "it", Nombre = "Italiano" },
            new Idioma { IdIdioma = "pt", Nombre = "Portugués" },
            new Idioma { IdIdioma = "ja", Nombre = "Japonés" },
            new Idioma { IdIdioma = "zh", Nombre = "Chino" }
        };

        foreach (var idioma in idiomas)
        {
            await session.RunAsync(
                "CREATE (i:Idioma {idIdioma: $idIdioma, nombre: $nombre})",
                new { idIdioma = idioma.IdIdioma, nombre = idioma.Nombre }
            );
        }

        var unidadesPorIdioma = new Dictionary<string, Unidad[]>
        {
            ["en"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_en", Nombre = "Básico 1", Posicion = 1, Descripcion = "Unidad introductoria de inglés", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_en", Nombre = "Básico 2", Posicion = 2, Descripcion = "Segunda unidad de inglés básico", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_en", Nombre = "Intermedio 1", Posicion = 3, Descripcion = "Primera unidad de inglés intermedio", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_en", Nombre = "Intermedio 2", Posicion = 4, Descripcion = "Segunda unidad de inglés intermedio", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_en", Nombre = "Avanzado 1", Posicion = 5, Descripcion = "Primera unidad de inglés avanzado", Nivel = "C1" }
            },
            ["es"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_es", Nombre = "Básico 1", Posicion = 1, Descripcion = "Unidad introductoria de español", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_es", Nombre = "Básico 2", Posicion = 2, Descripcion = "Segunda unidad de español básico", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_es", Nombre = "Intermedio 1", Posicion = 3, Descripcion = "Primera unidad de español intermedio", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_es", Nombre = "Intermedio 2", Posicion = 4, Descripcion = "Segunda unidad de español intermedio", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_es", Nombre = "Avanzado 1", Posicion = 5, Descripcion = "Primera unidad de español avanzado", Nivel = "C1" }
            },
            ["fr"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_fr", Nombre = "Débutant 1", Posicion = 1, Descripcion = "Unité introductive de français", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_fr", Nombre = "Débutant 2", Posicion = 2, Descripcion = "Deuxième unité de français débutant", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_fr", Nombre = "Intermédiaire 1", Posicion = 3, Descripcion = "Première unité de français intermédiaire", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_fr", Nombre = "Intermédiaire 2", Posicion = 4, Descripcion = "Deuxième unité de français intermédiaire", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_fr", Nombre = "Avancé 1", Posicion = 5, Descripcion = "Première unité de français avancé", Nivel = "C1" }
            },
            ["de"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_de", Nombre = "Anfänger 1", Posicion = 1, Descripcion = "Einführende Einheit Deutsch", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_de", Nombre = "Anfänger 2", Posicion = 2, Descripcion = "Zweite Einheit Deutsch für Anfänger", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_de", Nombre = "Mittelstufe 1", Posicion = 3, Descripcion = "Erste Einheit Deutsch Mittelstufe", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_de", Nombre = "Mittelstufe 2", Posicion = 4, Descripcion = "Zweite Einheit Deutsch Mittelstufe", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_de", Nombre = "Fortgeschritten 1", Posicion = 5, Descripcion = "Erste Einheit Deutsch Fortgeschritten", Nivel = "C1" }
            },
            ["it"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_it", Nombre = "Principiante 1", Posicion = 1, Descripcion = "Unità introduttiva di italiano", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_it", Nombre = "Principiante 2", Posicion = 2, Descripcion = "Seconda unità di italiano principiante", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_it", Nombre = "Intermedio 1", Posicion = 3, Descripcion = "Prima unità di italiano intermedio", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_it", Nombre = "Intermedio 2", Posicion = 4, Descripcion = "Seconda unità di italiano intermedio", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_it", Nombre = "Avanzato 1", Posicion = 5, Descripcion = "Prima unità di italiano avanzato", Nivel = "C1" }
            },
            ["pt"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_pt", Nombre = "Básico 1", Posicion = 1, Descripcion = "Unidade introdutória de português", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_pt", Nombre = "Básico 2", Posicion = 2, Descripcion = "Segunda unidade de português básico", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_pt", Nombre = "Intermediário 1", Posicion = 3, Descripcion = "Primeira unidade de português intermediário", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_pt", Nombre = "Intermediário 2", Posicion = 4, Descripcion = "Segunda unidade de português intermediário", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_pt", Nombre = "Avançado 1", Posicion = 5, Descripcion = "Primeira unidade de português avançado", Nivel = "C1" }
            },
            ["ja"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_ja", Nombre = "初級 1", Posicion = 1, Descripcion = "日本語入門ユニット", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_ja", Nombre = "初級 2", Posicion = 2, Descripcion = "日本語初級第二ユニット", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_ja", Nombre = "中級 1", Posicion = 3, Descripcion = "日本語中級第一ユニット", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_ja", Nombre = "中級 2", Posicion = 4, Descripcion = "日本語中級第二ユニット", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_ja", Nombre = "上級 1", Posicion = 5, Descripcion = "日本語上級第一ユニット", Nivel = "C1" }
            },
            ["zh"] = new[]
            {
                new Unidad { IdUnidad = "u_basic1_zh", Nombre = "初级 1", Posicion = 1, Descripcion = "中文入门单元", Nivel = "A1" },
                new Unidad { IdUnidad = "u_basic2_zh", Nombre = "初级 2", Posicion = 2, Descripcion = "中文初级第二单元", Nivel = "A1" },
                new Unidad { IdUnidad = "u_inter1_zh", Nombre = "中级 1", Posicion = 3, Descripcion = "中文中级第一单元", Nivel = "B1" },
                new Unidad { IdUnidad = "u_inter2_zh", Nombre = "中级 2", Posicion = 4, Descripcion = "中文中级第二单元", Nivel = "B2" },
                new Unidad { IdUnidad = "u_adv1_zh", Nombre = "高级 1", Posicion = 5, Descripcion = "中文高级第一单元", Nivel = "C1" }
            }
        };

        foreach (var idiomaEntry in unidadesPorIdioma)
        {
            var idiomaId = idiomaEntry.Key;
            var unidades = idiomaEntry.Value;
            
            foreach (var unidad in unidades)
            {
                await session.RunAsync(
                    "CREATE (u:Unidad {idUnidad: $idUnidad, nombre: $nombre, posicion: $posicion, descripcion: $descripcion, nivel: $nivel})",
                    new
                    {
                        idUnidad = unidad.IdUnidad,
                        nombre = unidad.Nombre,
                        posicion = unidad.Posicion,
                        descripcion = unidad.Descripcion,
                        nivel = unidad.Nivel
                    }
                );
                await session.RunAsync(
                    @"MATCH (u:Unidad {idUnidad: $unidadId}), (i:Idioma {idIdioma: $idiomaId})
                      CREATE (u)-[:DEL_IDIOMA]->(i)",
                    new { unidadId = unidad.IdUnidad, idiomaId = idiomaId }
                );
            }
        }

        var relacionesPrecedeA = new[]
        {
            // Inglés
            ("u_basic1_en", "u_basic2_en"), ("u_basic2_en", "u_inter1_en"), ("u_inter1_en", "u_inter2_en"), ("u_inter2_en", "u_adv1_en"),
            // Español
            ("u_basic1_es", "u_basic2_es"), ("u_basic2_es", "u_inter1_es"), ("u_inter1_es", "u_inter2_es"), ("u_inter2_es", "u_adv1_es"),
            // Francés
            ("u_basic1_fr", "u_basic2_fr"), ("u_basic2_fr", "u_inter1_fr"), ("u_inter1_fr", "u_inter2_fr"), ("u_inter2_fr", "u_adv1_fr"),
            // Alemán
            ("u_basic1_de", "u_basic2_de"), ("u_basic2_de", "u_inter1_de"), ("u_inter1_de", "u_inter2_de"), ("u_inter2_de", "u_adv1_de"),
            // Italiano
            ("u_basic1_it", "u_basic2_it"), ("u_basic2_it", "u_inter1_it"), ("u_inter1_it", "u_inter2_it"), ("u_inter2_it", "u_adv1_it"),
            // Portugués
            ("u_basic1_pt", "u_basic2_pt"), ("u_basic2_pt", "u_inter1_pt"), ("u_inter1_pt", "u_inter2_pt"), ("u_inter2_pt", "u_adv1_pt"),
            // Japonés
            ("u_basic1_ja", "u_basic2_ja"), ("u_basic2_ja", "u_inter1_ja"), ("u_inter1_ja", "u_inter2_ja"), ("u_inter2_ja", "u_adv1_ja"),
            // Chino
            ("u_basic1_zh", "u_basic2_zh"), ("u_basic2_zh", "u_inter1_zh"), ("u_inter1_zh", "u_inter2_zh"), ("u_inter2_zh", "u_adv1_zh")
        };

        foreach (var (unidadId1, unidadId2) in relacionesPrecedeA)
        {
            await session.RunAsync(
                @"MATCH (u1:Unidad {idUnidad: $unidadId1}), (u2:Unidad {idUnidad: $unidadId2})
                  CREATE (u1)-[:PRECEDE_A]->(u2)",
                new { unidadId1, unidadId2 }
            );
        }

        var habilidadesPorIdioma = new Dictionary<string, Habilidad[]>
        {
            ["en"] = new[]
            {
                new Habilidad { IdHabilidad = "h_present_simple", Nombre = "Present Simple", Descripcion = "Uso del present simple", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_past_simple", Nombre = "Past Simple", Descripcion = "Uso del past simple", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_future_will", Nombre = "Future with Will", Descripcion = "Uso del futuro con will", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_food", Nombre = "Vocabulario: Comida", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_travel", Nombre = "Vocabulario: Viajes", Descripcion = "Vocabulario relacionado con viajes", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_pronunciation", Nombre = "Pronunciación", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["es"] = new[]
            {
                new Habilidad { IdHabilidad = "h_ser_estar", Nombre = "Ser y Estar", Descripcion = "Diferencias entre ser y estar", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_pretérito", Nombre = "Pretérito", Descripcion = "Uso del pretérito perfecto", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_subjuntivo", Nombre = "Subjuntivo", Descripcion = "Uso del subjuntivo", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_familia", Nombre = "Vocabulario: Familia", Descripcion = "Vocabulario relacionado con familia", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_ciudad", Nombre = "Vocabulario: Ciudad", Descripcion = "Vocabulario relacionado con la ciudad", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_pronunciacion_es", Nombre = "Pronunciación", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["fr"] = new[]
            {
                new Habilidad { IdHabilidad = "h_present_fr", Nombre = "Présent", Descripcion = "Uso del presente en francés", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_passe_compose", Nombre = "Passé Composé", Descripcion = "Uso del passé composé", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_futur_fr", Nombre = "Futur", Descripcion = "Uso del futuro en francés", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_nourriture", Nombre = "Vocabulaire: Nourriture", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_maison", Nombre = "Vocabulaire: Maison", Descripcion = "Vocabulario relacionado con la casa", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_prononciation_fr", Nombre = "Prononciation", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["de"] = new[]
            {
                new Habilidad { IdHabilidad = "h_prasens", Nombre = "Präsens", Descripcion = "Uso del presente en alemán", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_perfekt", Nombre = "Perfekt", Descripcion = "Uso del perfecto en alemán", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_artikel", Nombre = "Artículos", Descripcion = "Uso de artículos der, die, das", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_essen", Nombre = "Wortschatz: Essen", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_familie", Nombre = "Wortschatz: Familie", Descripcion = "Vocabulario relacionado con familia", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_aussprache", Nombre = "Aussprache", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["it"] = new[]
            {
                new Habilidad { IdHabilidad = "h_presente", Nombre = "Presente", Descripcion = "Uso del presente en italiano", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_passato_prossimo", Nombre = "Passato Prossimo", Descripcion = "Uso del passato prossimo", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_articoli", Nombre = "Artículos", Descripcion = "Uso de artículos en italiano", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_cibo", Nombre = "Vocabolario: Cibo", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_citta", Nombre = "Vocabolario: Città", Descripcion = "Vocabulario relacionado con la ciudad", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_pronuncia", Nombre = "Pronuncia", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["pt"] = new[]
            {
                new Habilidad { IdHabilidad = "h_presente_pt", Nombre = "Presente", Descripcion = "Uso del presente en portugués", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_preterito_pt", Nombre = "Pretérito", Descripcion = "Uso del pretérito en portugués", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_futuro_pt", Nombre = "Futuro", Descripcion = "Uso del futuro en portugués", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_comida", Nombre = "Vocabulário: Comida", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_casa", Nombre = "Vocabulário: Casa", Descripcion = "Vocabulario relacionado con la casa", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_pronuncia_pt", Nombre = "Pronúncia", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["ja"] = new[]
            {
                new Habilidad { IdHabilidad = "h_hiragana", Nombre = "Hiragana", Descripcion = "Aprendizaje de hiragana", Categoria = "escritura" },
                new Habilidad { IdHabilidad = "h_katakana", Nombre = "Katakana", Descripcion = "Aprendizaje de katakana", Categoria = "escritura" },
                new Habilidad { IdHabilidad = "h_particulas", Nombre = "Partículas", Descripcion = "Uso de partículas japonesas", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_numeros", Nombre = "Vocabulario: Números", Descripcion = "Vocabulario de números", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_saludos", Nombre = "Vocabulario: Saludos", Descripcion = "Vocabulario de saludos", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_pronunciacion_ja", Nombre = "Pronunciación", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
            },
            ["zh"] = new[]
            {
                new Habilidad { IdHabilidad = "h_pinyin", Nombre = "Pinyin", Descripcion = "Aprendizaje de pinyin", Categoria = "escritura" },
                new Habilidad { IdHabilidad = "h_tonos", Nombre = "Tonos", Descripcion = "Uso de tonos en chino", Categoria = "pronunciación" },
                new Habilidad { IdHabilidad = "h_medidas", Nombre = "Medidas", Descripcion = "Uso de medidas en chino", Categoria = "gramática" },
                new Habilidad { IdHabilidad = "h_vocab_numeros_zh", Nombre = "Vocabulario: Números", Descripcion = "Vocabulario de números", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_vocab_familia_zh", Nombre = "Vocabulario: Familia", Descripcion = "Vocabulario relacionado con familia", Categoria = "vocabulario" },
                new Habilidad { IdHabilidad = "h_caracteres", Nombre = "Caracteres", Descripcion = "Aprendizaje de caracteres básicos", Categoria = "escritura" }
            }
        };

        foreach (var idiomaEntry in habilidadesPorIdioma)
        {
            var idiomaId = idiomaEntry.Key;
            var habilidades = idiomaEntry.Value;
            
            foreach (var habilidad in habilidades)
            {
                await session.RunAsync(
                    "CREATE (h:Habilidad {idHabilidad: $idHabilidad, nombre: $nombre, descripcion: $descripcion, categoria: $categoria})",
                    new
                    {
                        idHabilidad = habilidad.IdHabilidad,
                        nombre = habilidad.Nombre,
                        descripcion = habilidad.Descripcion,
                        categoria = habilidad.Categoria
                    }
                );
                await session.RunAsync(
                    @"MATCH (h:Habilidad {idHabilidad: $habilidadId}), (i:Idioma {idIdioma: $idiomaId})
                      CREATE (h)-[:DEL_IDIOMA]->(i)",
                    new { habilidadId = habilidad.IdHabilidad, idiomaId = idiomaId }
                );
            }
        }

        var ejerciciosPorUnidad = new Dictionary<string, (string nombre, string descripcion, string categoria)[]>
        {
            // Inglés - Básico 1
            ["u_basic1_en"] = new[] { ("Present Simple - Crear", "Crear oraciones en present simple", "crear"), ("Present Simple - Completar", "Completar verbos en present simple", "completar"), ("Present Simple - Traducir", "Traducir oraciones", "traducir") },
            // Inglés - Básico 2
            ["u_basic2_en"] = new[] { ("Past Simple - Regulares", "Conjugar verbos regulares", "conjugar"), ("Past Simple - Irregulares", "Conjugar verbos irregulares", "conjugar"), ("Past Simple - Preguntas", "Formar preguntas en past simple", "crear") },
            // Inglés - Intermedio 1
            ["u_inter1_en"] = new[] { ("Future Will - Ejercicio 1", "Usar will para futuro", "completar"), ("Future Going to", "Usar going to para futuro", "completar"), ("Vocabulario - Comida", "Matching de vocabulario", "matching") },
            // Inglés - Intermedio 2
            ["u_inter2_en"] = new[] { ("Present Perfect - Ejercicio 1", "Usar present perfect", "completar"), ("Present Perfect - Ejercicio 2", "Formar oraciones con present perfect", "crear"), ("Vocabulario - Viajes", "Vocabulario de viajes", "matching") },
            // Inglés - Avanzado 1
            ["u_adv1_en"] = new[] { ("Conditionals - Ejercicio 1", "Usar condicionales", "completar"), ("Passive Voice", "Transformar a voz pasiva", "transformar"), ("Pronunciación Avanzada", "Ejercicios de pronunciación", "pronunciación") },
            // Español - Básico 1
            ["u_basic1_es"] = new[] { ("Ser vs Estar 1", "Elegir entre ser y estar", "elegir"), ("Ser vs Estar 2", "Completar con ser o estar", "completar"), ("Artículos", "Usar artículos correctos", "completar") },
            // Español - Básico 2
            ["u_basic2_es"] = new[] { ("Pretérito - Conjugación", "Conjugar verbos en pretérito", "conjugar"), ("Pretérito - Usos", "Usar pretérito correctamente", "completar"), ("Vocabulario - Familia", "Vocabulario de familia", "matching") },
            // Español - Intermedio 1
            ["u_inter1_es"] = new[] { ("Subjuntivo - Presente", "Usar subjuntivo presente", "completar"), ("Subjuntivo - Ejercicio 2", "Formar oraciones con subjuntivo", "crear"), ("Vocabulario - Ciudad", "Vocabulario de ciudad", "matching") },
            // Español - Intermedio 2
            ["u_inter2_es"] = new[] { ("Condicional", "Usar condicional", "completar"), ("Imperfecto vs Pretérito", "Diferenciar imperfecto y pretérito", "elegir"), ("Expresiones Idiomáticas", "Usar expresiones idiomáticas", "completar") },
            // Español - Avanzado 1
            ["u_adv1_es"] = new[] { ("Subjuntivo Avanzado", "Subjuntivo en contextos complejos", "completar"), ("Voz Pasiva", "Transformar a voz pasiva", "transformar"), ("Pronunciación", "Ejercicios de pronunciación", "pronunciación") }
        };

        var todasLasUnidades = unidadesPorIdioma.Values.SelectMany(u => u).ToList();
        var ejercicioCounter = 1;
        var ejerciciosCreados = new List<(string ejercicioId, string unidadId, int posicion)>();

        foreach (var unidad in todasLasUnidades)
        {
            var ejercicios = ejerciciosPorUnidad.ContainsKey(unidad.IdUnidad) 
                ? ejerciciosPorUnidad[unidad.IdUnidad] 
                : new[] { 
                    ($"Ejercicio 1 - {unidad.Nombre}", $"Ejercicio básico de {unidad.Nombre}", "completar"),
                    ($"Ejercicio 2 - {unidad.Nombre}", $"Ejercicio de práctica de {unidad.Nombre}", "completar"),
                    ($"Ejercicio 3 - {unidad.Nombre}", $"Ejercicio avanzado de {unidad.Nombre}", "crear")
                };

            for (int i = 0; i < ejercicios.Length; i++)
            {
                var ejercicioId = $"e{ejercicioCounter:D3}";
                var (nombre, descripcion, categoria) = ejercicios[i];
                
                await session.RunAsync(
                    "CREATE (e:Ejercicio {idEjercicio: $idEjercicio, nombre: $nombre, descripcion: $descripcion, categoria: $categoria})",
                    new { idEjercicio = ejercicioId, nombre, descripcion, categoria }
                );
                
                ejerciciosCreados.Add((ejercicioId, unidad.IdUnidad, i + 1));
                ejercicioCounter++;
            }
        }

        foreach (var (ejercicioId, unidadId, posicion) in ejerciciosCreados)
        {
            await session.RunAsync(
                @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (u:Unidad {idUnidad: $unidadId})
                  CREATE (e)-[:PERTENECE_A {posicion: $posicion}]->(u)",
                new { ejercicioId, unidadId, posicion }
            );
        }

        var unidadHabilidadMap = new Dictionary<string, string[]>
        {
            ["u_basic1_en"] = new[] { "h_present_simple" },
            ["u_basic2_en"] = new[] { "h_past_simple" },
            ["u_inter1_en"] = new[] { "h_future_will", "h_vocab_food" },
            ["u_inter2_en"] = new[] { "h_vocab_travel" },
            ["u_adv1_en"] = new[] { "h_pronunciation" },
            ["u_basic1_es"] = new[] { "h_ser_estar" },
            ["u_basic2_es"] = new[] { "h_pretérito", "h_vocab_familia" },
            ["u_inter1_es"] = new[] { "h_subjuntivo", "h_vocab_ciudad" },
            ["u_inter2_es"] = new[] { "h_subjuntivo" },
            ["u_adv1_es"] = new[] { "h_pronunciacion_es" }
        };

        foreach (var (ejercicioId, unidadId, _) in ejerciciosCreados)
        {
            if (unidadHabilidadMap.ContainsKey(unidadId))
            {
                foreach (var habilidadId in unidadHabilidadMap[unidadId])
                {
                    await session.RunAsync(
                        @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (h:Habilidad {idHabilidad: $habilidadId})
                          CREATE (e)-[:REFUERZA]->(h)",
                        new { ejercicioId, habilidadId }
                    );
                }
            }
            else
            {
                var idiomaId = unidadId.Split('_').Last();
                var habilidadesDelIdioma = habilidadesPorIdioma.ContainsKey(idiomaId) 
                    ? habilidadesPorIdioma[idiomaId] 
                    : habilidadesPorIdioma["en"];
                if (habilidadesDelIdioma.Length > 0)
                {
                    await session.RunAsync(
                        @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (h:Habilidad {idHabilidad: $habilidadId})
                          CREATE (e)-[:REFUERZA]->(h)",
                        new { ejercicioId, habilidadId = habilidadesDelIdioma[0].IdHabilidad }
                    );
                }
            }
        }

        var relacionesEstudia = new[]
        {
            ("u1", "en", "A1", 2025, 1, 10), ("u1", "es", "A1", 2025, 3, 1),
            ("u2", "en", "A1", 2025, 2, 15), ("u2", "fr", "A1", 2025, 4, 1),
            ("u3", "en", "B1", 2024, 12, 1), ("u3", "de", "A2", 2025, 1, 5),
            ("u4", "es", "A1", 2025, 1, 20), ("u4", "pt", "A1", 2025, 2, 10),
            ("u5", "fr", "A2", 2024, 11, 15), ("u5", "it", "A1", 2025, 3, 1),
            ("u6", "en", "B2", 2024, 9, 1), ("u6", "es", "B1", 2024, 10, 15),
            ("u7", "de", "A1", 2025, 2, 1), ("u7", "en", "A2", 2025, 1, 10),
            ("u8", "it", "A1", 2025, 1, 15), ("u8", "es", "A1", 2025, 2, 20),
            ("u9", "pt", "A2", 2024, 12, 10), ("u9", "en", "B1", 2024, 11, 1),
            ("u10", "ja", "A1", 2025, 1, 5), ("u10", "zh", "A1", 2025, 2, 1),
            ("u11", "en", "A1", 2025, 3, 1), ("u11", "fr", "A1", 2025, 3, 15),
            ("u12", "es", "B1", 2024, 10, 1), ("u12", "pt", "A2", 2024, 11, 1),
            ("u13", "de", "A2", 2024, 12, 1), ("u13", "en", "B2", 2024, 9, 1),
            ("u14", "it", "A2", 2024, 11, 15), ("u14", "fr", "A1", 2025, 1, 1),
            ("u15", "zh", "A1", 2025, 2, 10), ("u15", "ja", "A1", 2025, 1, 20),
            ("u16", "en", "A1", 2025, 2, 1), ("u16", "es", "A1", 2025, 2, 15),
            ("u17", "fr", "B1", 2024, 10, 1), ("u17", "de", "A1", 2025, 1, 1),
            ("u18", "pt", "A1", 2025, 1, 10), ("u18", "es", "A2", 2024, 12, 1),
            ("u19", "it", "A1", 2025, 2, 5), ("u19", "en", "A1", 2025, 1, 1),
            ("u20", "ja", "A2", 2024, 11, 1), ("u20", "zh", "A1", 2025, 1, 1),
            ("u21", "en", "A2", 2024, 12, 1), ("u21", "de", "A1", 2025, 2, 1),
            ("u22", "es", "A1", 2025, 1, 15), ("u22", "pt", "A1", 2025, 3, 1),
            ("u23", "fr", "A1", 2025, 2, 10), ("u23", "it", "A1", 2025, 1, 20),
            ("u24", "zh", "A1", 2025, 1, 5), ("u24", "ja", "A1", 2025, 2, 15),
            ("u25", "en", "B1", 2024, 10, 1), ("u25", "es", "A2", 2024, 11, 1),
            ("u26", "de", "A1", 2025, 3, 1), ("u26", "fr", "A1", 2025, 1, 10),
            ("u27", "pt", "A2", 2024, 12, 15), ("u27", "es", "B1", 2024, 9, 1),
            ("u28", "it", "A1", 2025, 2, 1), ("u28", "en", "A1", 2025, 1, 5),
            ("u29", "ja", "A1", 2025, 1, 10), ("u29", "zh", "A1", 2025, 2, 5),
            ("u30", "en", "A1", 2025, 2, 20), ("u30", "fr", "A1", 2025, 3, 1)
        };

        foreach (var (usuarioId, idiomaId, nivel, year, month, day) in relacionesEstudia)
        {
            await session.RunAsync(
                @"MATCH (u:Usuario {idUsuario: $usuarioId}), (i:Idioma {idIdioma: $idiomaId})
                  CREATE (u)-[:ESTUDIA {nivel: $nivel, fechaInicio: $fechaInicio, fechaUltimaActividad: $fechaUltimaActividad}]->(i)",
                new
                {
                    usuarioId,
                    idiomaId,
                    nivel,
                    fechaInicio = new DateTime(year, month, day).ToString("yyyy-MM-dd"),
                    fechaUltimaActividad = DateTime.Now.ToString("yyyy-MM-dd")
                }
            );
        }

        var random = new Random(42); 
        var primerosEjercicios = ejerciciosCreados.Take(50).ToList();
        
        for (int i = 1; i <= 30; i++)
        {
            var usuarioId = $"u{i}";
            var ejerciciosUsuario = primerosEjercicios.OrderBy(x => random.Next()).Take(random.Next(3, 8)).ToList();
            
            foreach (var (ejercicioId, _, _) in ejerciciosUsuario)
            {
                var resultado = random.Next(0, 100) < 70 ? "correcto" : "incorrecto";
                var tiempo = random.Next(20, 60);
                var intentos = resultado == "incorrecto" ? random.Next(1, 3) : 1;
                var diasAtras = random.Next(1, 30);
                
                await session.RunAsync(
                    @"MATCH (u:Usuario {idUsuario: $usuarioId}), (e:Ejercicio {idEjercicio: $ejercicioId})
                      CREATE (u)-[:REALIZA {fechaRealizado: $fechaRealizado, resultado: $resultado, tiempo: $tiempo, intentos: $intentos}]->(e)",
                    new
                    {
                        usuarioId,
                        ejercicioId,
                        fechaRealizado = DateTime.Now.AddDays(-diasAtras).ToString("yyyy-MM-ddTHH:mm:ss"),
                        resultado,
                        tiempo,
                        intentos
                    }
                );
            }
        }

        var todasLasHabilidades = habilidadesPorIdioma.Values.SelectMany(h => h).Select(h => h.IdHabilidad).ToList();
        var relacionesFallaEn = new[]
        {
            ("u1", "h_present_simple", 5), ("u1", "h_vocab_food", 3),
            ("u2", "h_past_simple", 2), ("u2", "h_future_will", 4),
            ("u3", "h_vocab_travel", 1), ("u4", "h_ser_estar", 6),
            ("u5", "h_pretérito", 3), ("u6", "h_subjuntivo", 5),
            ("u7", "h_pronunciacion_es", 2), ("u8", "h_present_fr", 4),
            ("u9", "h_passe_compose", 3), ("u10", "h_prasens", 5),
            ("u11", "h_perfekt", 2), ("u12", "h_presente", 4),
            ("u13", "h_passato_prossimo", 3), ("u14", "h_presente_pt", 5),
            ("u15", "h_hiragana", 7), ("u16", "h_katakana", 4),
            ("u17", "h_pinyin", 6), ("u18", "h_tonos", 8)
        };

        foreach (var (usuarioId, habilidadId, vecesFalladas) in relacionesFallaEn)
        {
            await session.RunAsync(
                @"MATCH (u:Usuario {idUsuario: $usuarioId}), (h:Habilidad {idHabilidad: $habilidadId})
                  CREATE (u)-[:FALLA_EN {vecesFalladas: $vecesFalladas}]->(h)",
                new { usuarioId, habilidadId, vecesFalladas }
            );
        }

        var relacionesSimilarA = new[]
        {
            ("u1", "u2"), ("u2", "u1"), ("u2", "u3"), ("u3", "u2"),
            ("u4", "u5"), ("u5", "u4"), ("u6", "u7"), ("u7", "u6"),
            ("u8", "u9"), ("u9", "u8"), ("u10", "u11"), ("u11", "u10"),
            ("u12", "u13"), ("u13", "u12"), ("u14", "u15"), ("u15", "u14"),
            ("u16", "u17"), ("u17", "u16"), ("u18", "u19"), ("u19", "u18"),
            ("u20", "u21"), ("u21", "u20"), ("u22", "u23"), ("u23", "u22"),
            ("u24", "u25"), ("u25", "u24"), ("u26", "u27"), ("u27", "u26"),
            ("u28", "u29"), ("u29", "u28"), ("u30", "u1"), ("u1", "u30")
        };

        foreach (var (usuarioId1, usuarioId2) in relacionesSimilarA)
        {
            await session.RunAsync(
                @"MATCH (u1:Usuario {idUsuario: $usuarioId1}), (u2:Usuario {idUsuario: $usuarioId2})
                  CREATE (u1)-[:SIMILAR_A]->(u2)",
                new { usuarioId1, usuarioId2 }
            );
        }
    }

    #endregion
}

