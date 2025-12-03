using Neo4j.Driver;
using ObligatorioBDNR.Data.Neo4j;
using ObligatorioBDNR.Models.Recomendacion;

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
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            "CREATE (u:Usuario {idUsuario: $idUsuario, username: $username})",
            new { idUsuario = usuario.IdUsuario, username = usuario.Username }
        );
    }

    public async Task CrearIdiomaAsync(Idioma idioma)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            "CREATE (i:Idioma {idIdioma: $idIdioma, nombre: $nombre})",
            new { idIdioma = idioma.IdIdioma, nombre = idioma.Nombre }
        );
    }

    public async Task CrearUnidadAsync(Unidad unidad)
    {
        await using var session = _context.Driver.AsyncSession();
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
    }

    public async Task CrearHabilidadAsync(Habilidad habilidad)
    {
        await using var session = _context.Driver.AsyncSession();
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
    }

    public async Task CrearEjercicioAsync(Ejercicio ejercicio)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            "CREATE (e:Ejercicio {idEjercicio: $idEjercicio, nombre: $nombre, descripcion: $descripcion, categoria: $categoria})",
            new
            {
                idEjercicio = ejercicio.IdEjercicio,
                nombre = ejercicio.Nombre,
                descripcion = ejercicio.Descripcion,
                categoria = ejercicio.Categoria
            }
        );
    }

    public async Task CrearRelacionEstudiaAsync(RelacionEstudia relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (u)-[:ESTUDIA {nivel: $nivel, fechaInicio: $fechaInicio, fechaUltimaActividad: $fechaUltimaActividad}]->(i)",
            new
            {
                usuarioId = relacion.UsuarioId,
                idiomaId = relacion.IdiomaId,
                nivel = relacion.Nivel,
                fechaInicio = relacion.FechaInicio.ToString("yyyy-MM-dd"),
                fechaUltimaActividad = relacion.FechaUltimaActividad.ToString("yyyy-MM-dd")
            }
        );
    }

    public async Task CrearRelacionDelIdiomaUnidadAsync(string unidadId, string idiomaId)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u:Unidad {idUnidad: $unidadId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (u)-[:DEL_IDIOMA]->(i)",
            new { unidadId, idiomaId }
        );
    }

    public async Task CrearRelacionDelIdiomaHabilidadAsync(string habilidadId, string idiomaId)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (h:Habilidad {idHabilidad: $habilidadId}), (i:Idioma {idIdioma: $idiomaId})
              CREATE (h)-[:DEL_IDIOMA]->(i)",
            new { habilidadId, idiomaId }
        );
    }

    public async Task CrearRelacionPerteneceAAsync(RelacionPerteneceA relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (u:Unidad {idUnidad: $unidadId})
              CREATE (e)-[:PERTENECE_A {posicion: $posicion}]->(u)",
            new
            {
                ejercicioId = relacion.EjercicioId,
                unidadId = relacion.UnidadId,
                posicion = relacion.Posicion
            }
        );
    }

    public async Task CrearRelacionRefuerzaAsync(string ejercicioId, string habilidadId)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (e:Ejercicio {idEjercicio: $ejercicioId}), (h:Habilidad {idHabilidad: $habilidadId})
              CREATE (e)-[:REFUERZA]->(h)",
            new { ejercicioId, habilidadId }
        );
    }

    public async Task CrearRelacionRealizaAsync(RelacionRealiza relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (e:Ejercicio {idEjercicio: $ejercicioId})
              CREATE (u)-[:REALIZA {fechaRealizado: $fechaRealizado, resultado: $resultado, tiempo: $tiempo, intentos: $intentos}]->(e)",
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
    }

    public async Task CrearRelacionFallaEnAsync(RelacionFallaEn relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u:Usuario {idUsuario: $usuarioId}), (h:Habilidad {idHabilidad: $habilidadId})
              CREATE (u)-[:FALLA_EN {vecesFalladas: $vecesFalladas}]->(h)",
            new
            {
                usuarioId = relacion.UsuarioId,
                habilidadId = relacion.HabilidadId,
                vecesFalladas = relacion.VecesFalladas
            }
        );
    }

    public async Task CrearRelacionSimilarAAsync(RelacionSimilarA relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u1:Usuario {idUsuario: $usuarioId1}), (u2:Usuario {idUsuario: $usuarioId2})
              CREATE (u1)-[:SIMILAR_A]->(u2)",
            new { usuarioId1 = relacion.UsuarioId1, usuarioId2 = relacion.UsuarioId2 }
        );
    }

    public async Task CrearRelacionPrecedeAAsync(RelacionPrecedeA relacion)
    {
        await using var session = _context.Driver.AsyncSession();
        await session.RunAsync(
            @"MATCH (u1:Unidad {idUnidad: $unidadId1}), (u2:Unidad {idUnidad: $unidadId2})
              CREATE (u1)-[:PRECEDE_A]->(u2)",
            new { unidadId1 = relacion.UnidadId1, unidadId2 = relacion.UnidadId2 }
        );
    }

    #endregion

    #region Métodos auxiliares para GDS

    private async Task CrearProyeccionUsuariosEjerciciosAsync()
    {
        await using var session = _context.Driver.AsyncSession();
        
        // Verificar si la proyección ya existe
        var checkResult = await session.RunAsync("CALL gds.graph.exists('usuarios-ejercicios') YIELD exists RETURN exists");
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = record["exists"].As<bool>();
        }

        if (exists)
        {
            return; // La proyección ya existe
        }

        // Eliminar proyección si existe (por si acaso)
        try
        {
            await session.RunAsync("CALL gds.graph.drop('usuarios-ejercicios', false)");
        }
        catch { }

        // Crear proyección para similitud de usuarios basada en ejercicios realizados
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
            // Si falla, puede ser que no haya suficientes datos
            throw new Exception($"Error al crear proyección GDS 'usuarios-ejercicios': {ex.Message}");
        }
    }

    private async Task CrearProyeccionEjerciciosHabilidadesAsync()
    {
        await using var session = _context.Driver.AsyncSession();
        
        // Verificar si la proyección ya existe
        var checkResult = await session.RunAsync("CALL gds.graph.exists('ejercicios-habilidades') YIELD exists RETURN exists");
        var exists = false;
        await foreach (var record in checkResult)
        {
            exists = record["exists"].As<bool>();
        }

        if (exists)
        {
            return; // La proyección ya existe
        }

        try
        {
            await session.RunAsync("CALL gds.graph.drop('ejercicios-habilidades', false)");
        }
        catch { }

        // Crear proyección para encontrar ejercicios importantes basados en habilidades
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
        await using var session = _context.Driver.AsyncSession();
        
        // Verificar si la proyección ya existe
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
                return; // La proyección ya existe
            }
        }
        catch
        {
            // Si gds.graph.exists no está disponible, continuar con la creación
        }

        // Eliminar proyección si existe (por si acaso)
        try
        {
            await session.RunAsync("CALL gds.graph.drop('usuarios-habilidades', false)");
        }
        catch { }

        // Crear proyección para similitud basada en dificultades (habilidades donde fallan)
        // Nota: Esta proyección puede no ser necesaria para el Patrón 1, pero la mantenemos por si se necesita en el futuro
        try
        {
            // Primero intentar con sintaxis estándar
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
            // Si falla, puede ser que no haya suficientes datos o que la sintaxis sea diferente
            // En este caso, simplemente no creamos la proyección y continuamos
            // El método de recomendación puede funcionar sin esta proyección específica
            System.Diagnostics.Debug.WriteLine($"Advertencia: No se pudo crear proyección 'usuarios-habilidades': {ex.Message}");
        }
    }

    #endregion

    #region Métodos para consultar recomendaciones

    // Patrón 1: Recomendación basada en las dificultades del usuario (usando GDS)
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorDificultadesAsync(string usuarioId)
    {
        var resultados = new List<RecomendacionResultado>();

        try
        {
            await using var session = _context.Driver.AsyncSession();
            
            // Consulta simplificada y más robusta
            // Busca ejercicios que refuercen habilidades donde el usuario tiene dificultades
            var result = await session.RunAsync(
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

            await foreach (var record in result)
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
                    TipoRecomendacion = "Basada en dificultades del usuario (GDS)"
                });
            }
        }
        catch (Exception ex)
        {
            // Si hay un error, lanzar una excepción más descriptiva
            throw new Exception($"Error al obtener recomendaciones por dificultades para usuario {usuarioId}: {ex.Message}", ex);
        }

        return resultados;
    }

    // Patrón 2: Filtro basado en la similitud de usuarios (usando GDS Node Similarity)
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorSimilitudUsuariosAsync(string usuarioId)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.Driver.AsyncSession();
        
        // Asegurar que la proyección existe
        await CrearProyeccionUsuariosEjerciciosAsync();

        // Paso 1: Calcular similitud de usuarios usando GDS Node Similarity
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

        // Si no hay usuarios similares, usar fallback a relaciones SIMILAR_A manuales
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

        // Paso 2: Obtener ejercicios de usuarios similares con score ponderado
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

    // Patrón 3: Recomendación basada en el contenido y la estructura del curso (usando GDS PageRank)
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorContenidoCursoAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.Driver.AsyncSession();
        
        // Asegurar que la proyección existe
        await CrearProyeccionEjerciciosHabilidadesAsync();

        // Usar PageRank para encontrar ejercicios importantes en la estructura del curso
        string graphName = "ejercicios-habilidades";
        
        // Ejecutar PageRank en la proyección
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

        var ejerciciosScores = new Dictionary<string, double>();
        await foreach (var record in pageRankResult)
        {
            ejerciciosScores[record["ejercicio"].As<string>()] = record["score"].As<double>();
        }

        // Combinar con información del usuario y estructura del curso
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

        // Ordenar por score de PageRank
        resultados = resultados.OrderByDescending(r => r.Score).ThenBy(r => 
        {
            var posicionStr = r.Unidad?.Split(' ').LastOrDefault() ?? "0";
            return int.TryParse(posicionStr, out var pos) ? pos : 999;
        }).Take(20).ToList();

        return resultados;
    }

    // Patrón 4: Recomendación según el idioma estudiado por el usuario (usando GDS)
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesPorIdiomaAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.Driver.AsyncSession();
        
        // Usar GDS para encontrar ejercicios importantes en el contexto del idioma
        await CrearProyeccionEjerciciosHabilidadesAsync();

        // Usar Betweenness Centrality para encontrar ejercicios centrales en el grafo del idioma
        var centralityResult = await session.RunAsync(@"
            CALL gds.betweenness.stream('ejercicios-habilidades', {
                maxDepth: 3
            })
            YIELD nodeId, score
            MATCH (e:Ejercicio) WHERE id(e) = nodeId
            RETURN e.idEjercicio AS ejercicio, score
            ORDER BY score DESC
            LIMIT 50",
            new { }
        );

        var ejerciciosScores = new Dictionary<string, double>();
        await foreach (var record in centralityResult)
        {
            ejerciciosScores[record["ejercicio"].As<string>()] = record["score"].As<double>();
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

        // Ordenar por score de centralidad
        resultados = resultados.OrderByDescending(r => r.Score).ThenBy(r => 
        {
            var posicionStr = r.Unidad?.Split(' ').LastOrDefault() ?? "0";
            return int.TryParse(posicionStr, out var pos) ? pos : 999;
        }).Take(20).ToList();

        return resultados;
    }

    // Patrón 5: Consultas según diversos criterios (combinado usando múltiples algoritmos GDS)
    public async Task<List<RecomendacionResultado>> ObtenerRecomendacionesCombinadasAsync(string usuarioId, string? idiomaId = null)
    {
        var resultados = new List<RecomendacionResultado>();

        await using var session = _context.Driver.AsyncSession();
        
        // Asegurar que las proyecciones existen
        await CrearProyeccionUsuariosEjerciciosAsync();
        await CrearProyeccionEjerciciosHabilidadesAsync();
        await CrearProyeccionUsuariosHabilidadesAsync();

        // 1. Obtener similitud de usuarios usando GDS
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

        // 2. Obtener PageRank de ejercicios
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

        // 3. Combinar todos los criterios en una consulta
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
                       idI.nombre AS idioma,
                       (vecesFalladas * 2.0 + similarScore * 1.5) AS baseScore
                ORDER BY baseScore DESC, un.posicion ASC
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
                       idI.nombre AS idioma,
                       (vecesFalladas * 2.0 + similarScore * 1.5) AS baseScore
                ORDER BY baseScore DESC, un.posicion ASC
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
            
            // Combinar scores: baseScore (dificultades + similitud) + PageRank normalizado
            var combinedScore = baseScore + (pageRankScore * 10.0); // Normalizar PageRank

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
            await using var session = _context.Driver.AsyncSession();
            
            var result = await session.RunAsync(@"
                MATCH (u:Usuario)
                OPTIONAL MATCH (u)-[:ESTUDIA]->(i:Idioma)
                OPTIONAL MATCH (u)-[:FALLA_EN]->(h:Habilidad)
                OPTIONAL MATCH (e:Ejercicio)
                RETURN count(DISTINCT u) AS usuarios,
                       count(DISTINCT i) AS idiomas,
                       count(DISTINCT h) AS habilidades,
                       count(DISTINCT e) AS ejercicios,
                       count((u)-[:ESTUDIA]->()) AS relacionesEstudia,
                       count((u)-[:FALLA_EN]->()) AS relacionesFallaEn"
            );

            var record = await result.SingleAsync();
            var usuarios = record["usuarios"].As<int>();
            var idiomas = record["idiomas"].As<int>();
            var habilidades = record["habilidades"].As<int>();
            var ejercicios = record["ejercicios"].As<int>();
            var relacionesEstudia = record["relacionesEstudia"].As<int>();

            return usuarios > 0 && idiomas > 0 && habilidades > 0 && ejercicios > 0 && relacionesEstudia > 0;
        }
        catch (Exception ex)
        {
            // Si hay un error, probablemente no hay datos o hay un problema de conexión
            System.Diagnostics.Debug.WriteLine($"Error al verificar datos: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Método para cargar datos de ejemplo

    public async Task CargarDatosEjemploAsync()
    {
        await using var session = _context.Driver.AsyncSession();

        await session.RunAsync("MATCH (n) DETACH DELETE n");

        var usuarios = new[]
        {
            new Usuario { IdUsuario = "u1", Username = "Laura" },
            new Usuario { IdUsuario = "u2", Username = "Carlos" },
            new Usuario { IdUsuario = "u3", Username = "Ana" },
            new Usuario { IdUsuario = "u4", Username = "Pedro" }
        };

        foreach (var usuario in usuarios)
        {
            await CrearUsuarioAsync(usuario);
        }

        var idiomas = new[]
        {
            new Idioma { IdIdioma = "en", Nombre = "Inglés" },
            new Idioma { IdIdioma = "es", Nombre = "Español" },
            new Idioma { IdIdioma = "fr", Nombre = "Francés" }
        };

        foreach (var idioma in idiomas)
        {
            await CrearIdiomaAsync(idioma);
        }

        var unidadesEn = new[]
        {
            new Unidad { IdUnidad = "u_basic1_en", Nombre = "Básico 1", Posicion = 1, Descripcion = "Unidad introductoria de inglés", Nivel = "A1" },
            new Unidad { IdUnidad = "u_basic2_en", Nombre = "Básico 2", Posicion = 2, Descripcion = "Segunda unidad de inglés básico", Nivel = "A1" },
            new Unidad { IdUnidad = "u_inter1_en", Nombre = "Intermedio 1", Posicion = 3, Descripcion = "Primera unidad de inglés intermedio", Nivel = "B1" }
        };

        foreach (var unidad in unidadesEn)
        {
            await CrearUnidadAsync(unidad);
            await CrearRelacionDelIdiomaUnidadAsync(unidad.IdUnidad, "en");
        }

        var unidadesEs = new[]
        {
            new Unidad { IdUnidad = "u_basic1_es", Nombre = "Básico 1", Posicion = 1, Descripcion = "Unidad introductoria de español", Nivel = "A1" },
            new Unidad { IdUnidad = "u_basic2_es", Nombre = "Básico 2", Posicion = 2, Descripcion = "Segunda unidad de español básico", Nivel = "A1" }
        };

        foreach (var unidad in unidadesEs)
        {
            await CrearUnidadAsync(unidad);
            await CrearRelacionDelIdiomaUnidadAsync(unidad.IdUnidad, "es");
        }

        await CrearRelacionPrecedeAAsync(new RelacionPrecedeA { UnidadId1 = "u_basic1_en", UnidadId2 = "u_basic2_en" });
        await CrearRelacionPrecedeAAsync(new RelacionPrecedeA { UnidadId1 = "u_basic2_en", UnidadId2 = "u_inter1_en" });
        await CrearRelacionPrecedeAAsync(new RelacionPrecedeA { UnidadId1 = "u_basic1_es", UnidadId2 = "u_basic2_es" });

        var habilidadesEn = new[]
        {
            new Habilidad { IdHabilidad = "h_present_simple", Nombre = "Present Simple", Descripcion = "Uso del present simple en una oración", Categoria = "gramática" },
            new Habilidad { IdHabilidad = "h_past_simple", Nombre = "Past Simple", Descripcion = "Uso del past simple", Categoria = "gramática" },
            new Habilidad { IdHabilidad = "h_vocab_food", Nombre = "Vocabulario: Comida", Descripcion = "Vocabulario relacionado con comida", Categoria = "vocabulario" },
            new Habilidad { IdHabilidad = "h_pronunciation", Nombre = "Pronunciación", Descripcion = "Mejora de la pronunciación", Categoria = "pronunciación" }
        };

        foreach (var habilidad in habilidadesEn)
        {
            await CrearHabilidadAsync(habilidad);
            await CrearRelacionDelIdiomaHabilidadAsync(habilidad.IdHabilidad, "en");
        }

        var habilidadesEs = new[]
        {
            new Habilidad { IdHabilidad = "h_ser_estar", Nombre = "Ser y Estar", Descripcion = "Diferencias entre ser y estar", Categoria = "gramática" },
            new Habilidad { IdHabilidad = "h_pretérito", Nombre = "Pretérito", Descripcion = "Uso del pretérito perfecto", Categoria = "gramática" }
        };

        foreach (var habilidad in habilidadesEs)
        {
            await CrearHabilidadAsync(habilidad);
            await CrearRelacionDelIdiomaHabilidadAsync(habilidad.IdHabilidad, "es");
        }

        var ejerciciosEn = new[]
        {
            new Ejercicio { IdEjercicio = "e101", Nombre = "Crear oraciones present simple 1", Descripcion = "Crear oraciones negativas en present simple", Categoria = "crear" },
            new Ejercicio { IdEjercicio = "e102", Nombre = "Completar present simple 1", Descripcion = "Completar los verbos en present simple", Categoria = "completar" },
            new Ejercicio { IdEjercicio = "e103", Nombre = "Traducir present simple", Descripcion = "Traducir oraciones al present simple", Categoria = "traducir" },
            new Ejercicio { IdEjercicio = "e201", Nombre = "Past simple - Verbos regulares", Descripcion = "Conjugar verbos regulares en past simple", Categoria = "conjugar" },
            new Ejercicio { IdEjercicio = "e202", Nombre = "Past simple - Verbos irregulares", Descripcion = "Conjugar verbos irregulares en past simple", Categoria = "conjugar" },
            new Ejercicio { IdEjercicio = "e301", Nombre = "Vocabulario comida - Matching", Descripcion = "Emparejar palabras de comida con imágenes", Categoria = "matching" },
            new Ejercicio { IdEjercicio = "e302", Nombre = "Vocabulario comida - Listening", Descripcion = "Escuchar y escribir palabras de comida", Categoria = "listening" },
            new Ejercicio { IdEjercicio = "e401", Nombre = "Pronunciación - Vocales", Descripcion = "Practicar pronunciación de vocales", Categoria = "pronunciación" }
        };

        foreach (var ejercicio in ejerciciosEn)
        {
            await CrearEjercicioAsync(ejercicio);
        }

        var ejerciciosEs = new[]
        {
            new Ejercicio { IdEjercicio = "e501", Nombre = "Ser vs Estar - Ejercicio 1", Descripcion = "Elegir entre ser y estar", Categoria = "elegir" },
            new Ejercicio { IdEjercicio = "e502", Nombre = "Ser vs Estar - Ejercicio 2", Descripcion = "Completar con ser o estar", Categoria = "completar" },
            new Ejercicio { IdEjercicio = "e601", Nombre = "Pretérito - Conjugación", Descripcion = "Conjugar verbos en pretérito", Categoria = "conjugar" }
        };

        foreach (var ejercicio in ejerciciosEs)
        {
            await CrearEjercicioAsync(ejercicio);
        }

        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e101", UnidadId = "u_basic1_en", Posicion = 1 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e102", UnidadId = "u_basic1_en", Posicion = 2 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e103", UnidadId = "u_basic1_en", Posicion = 3 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e201", UnidadId = "u_basic2_en", Posicion = 1 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e202", UnidadId = "u_basic2_en", Posicion = 2 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e301", UnidadId = "u_basic1_en", Posicion = 4 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e302", UnidadId = "u_basic1_en", Posicion = 5 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e401", UnidadId = "u_inter1_en", Posicion = 1 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e501", UnidadId = "u_basic1_es", Posicion = 1 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e502", UnidadId = "u_basic1_es", Posicion = 2 });
        await CrearRelacionPerteneceAAsync(new RelacionPerteneceA { EjercicioId = "e601", UnidadId = "u_basic2_es", Posicion = 1 });

        await CrearRelacionRefuerzaAsync("e101", "h_present_simple");
        await CrearRelacionRefuerzaAsync("e102", "h_present_simple");
        await CrearRelacionRefuerzaAsync("e103", "h_present_simple");
        await CrearRelacionRefuerzaAsync("e201", "h_past_simple");
        await CrearRelacionRefuerzaAsync("e202", "h_past_simple");
        await CrearRelacionRefuerzaAsync("e301", "h_vocab_food");
        await CrearRelacionRefuerzaAsync("e302", "h_vocab_food");
        await CrearRelacionRefuerzaAsync("e401", "h_pronunciation");
        await CrearRelacionRefuerzaAsync("e501", "h_ser_estar");
        await CrearRelacionRefuerzaAsync("e502", "h_ser_estar");
        await CrearRelacionRefuerzaAsync("e601", "h_pretérito");

        await CrearRelacionEstudiaAsync(new RelacionEstudia
        {
            UsuarioId = "u1",
            IdiomaId = "en",
            Nivel = "A1",
            FechaInicio = new DateTime(2025, 1, 10),
            FechaUltimaActividad = DateTime.Now
        });

        await CrearRelacionEstudiaAsync(new RelacionEstudia
        {
            UsuarioId = "u2",
            IdiomaId = "en",
            Nivel = "A1",
            FechaInicio = new DateTime(2025, 2, 15),
            FechaUltimaActividad = DateTime.Now
        });

        await CrearRelacionEstudiaAsync(new RelacionEstudia
        {
            UsuarioId = "u3",
            IdiomaId = "en",
            Nivel = "B1",
            FechaInicio = new DateTime(2024, 12, 1),
            FechaUltimaActividad = DateTime.Now
        });

        await CrearRelacionEstudiaAsync(new RelacionEstudia
        {
            UsuarioId = "u1",
            IdiomaId = "es",
            Nivel = "A1",
            FechaInicio = new DateTime(2025, 3, 1),
            FechaUltimaActividad = DateTime.Now
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u1",
            EjercicioId = "e101",
            FechaRealizado = DateTime.Now.AddDays(-5),
            Resultado = "incorrecto",
            Tiempo = 40,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u1",
            EjercicioId = "e102",
            FechaRealizado = DateTime.Now.AddDays(-3),
            Resultado = "correcto",
            Tiempo = 30,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u2",
            EjercicioId = "e101",
            FechaRealizado = DateTime.Now.AddDays(-2),
            Resultado = "correcto",
            Tiempo = 35,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u2",
            EjercicioId = "e201",
            FechaRealizado = DateTime.Now.AddDays(-1),
            Resultado = "correcto",
            Tiempo = 45,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u3",
            EjercicioId = "e201",
            FechaRealizado = DateTime.Now.AddDays(-4),
            Resultado = "correcto",
            Tiempo = 30,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u3",
            EjercicioId = "e202",
            FechaRealizado = DateTime.Now.AddDays(-2),
            Resultado = "correcto",
            Tiempo = 35,
            Intentos = 1
        });

        await CrearRelacionRealizaAsync(new RelacionRealiza
        {
            UsuarioId = "u1",
            EjercicioId = "e301",
            FechaRealizado = DateTime.Now.AddDays(-1),
            Resultado = "incorrecto",
            Tiempo = 50,
            Intentos = 2
        });

        await CrearRelacionFallaEnAsync(new RelacionFallaEn
        {
            UsuarioId = "u1",
            HabilidadId = "h_present_simple",
            VecesFalladas = 5
        });

        await CrearRelacionFallaEnAsync(new RelacionFallaEn
        {
            UsuarioId = "u1",
            HabilidadId = "h_vocab_food",
            VecesFalladas = 3
        });

        await CrearRelacionFallaEnAsync(new RelacionFallaEn
        {
            UsuarioId = "u2",
            HabilidadId = "h_past_simple",
            VecesFalladas = 2
        });

        await CrearRelacionSimilarAAsync(new RelacionSimilarA { UsuarioId1 = "u1", UsuarioId2 = "u2" });
        await CrearRelacionSimilarAAsync(new RelacionSimilarA { UsuarioId1 = "u2", UsuarioId2 = "u1" });
        await CrearRelacionSimilarAAsync(new RelacionSimilarA { UsuarioId1 = "u2", UsuarioId2 = "u3" });
        await CrearRelacionSimilarAAsync(new RelacionSimilarA { UsuarioId1 = "u3", UsuarioId2 = "u2" });
    }

    #endregion
}

