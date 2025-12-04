using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

/// <summary>
/// Repositorio para gestionar operaciones CRUD de estadísticas de usuarios
/// </summary>
public class EstadisticaUsuarioRepository
{
    private readonly IMongoCollection<EstadisticaUsuario> _collection;

    public EstadisticaUsuarioRepository(MongoContext context)
    {
        _collection = context.Estadisticas;
    }

    /// <summary>
    /// Obtiene todas las estadísticas
    /// </summary>
    public async Task<List<EstadisticaUsuario>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Obtiene una estadística por su ID
    /// </summary>
    public async Task<EstadisticaUsuario?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene estadísticas de un usuario
    /// </summary>
    public async Task<List<EstadisticaUsuario>> GetByUsuarioIdAsync(Guid idUsuario)
    {
        return await _collection
            .Find(e => e.IdUsuario == idUsuario)
            .SortByDescending(e => e.Fecha)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene estadísticas de un usuario por rango de fechas
    /// </summary>
    public async Task<List<EstadisticaUsuario>> GetByUsuarioAndDateRangeAsync(
        Guid idUsuario,
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        var filter = Builders<EstadisticaUsuario>.Filter.And(
            Builders<EstadisticaUsuario>.Filter.Eq(e => e.IdUsuario, idUsuario),
            Builders<EstadisticaUsuario>.Filter.Gte(e => e.Fecha, fechaInicio.Date),
            Builders<EstadisticaUsuario>.Filter.Lte(e => e.Fecha, fechaFin.Date)
        );
        return await _collection
            .Find(filter)
            .SortByDescending(e => e.Fecha)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene estadísticas de un usuario por fecha específica
    /// </summary>
    public async Task<EstadisticaUsuario?> GetByUsuarioAndDateAsync(Guid idUsuario, DateTime fecha)
    {
        var fechaInicio = fecha.Date;
        var fechaFin = fecha.Date.AddDays(1);
        
        return await _collection
            .Find(e => e.IdUsuario == idUsuario &&
                       e.Fecha >= fechaInicio &&
                       e.Fecha < fechaFin)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene estadísticas paginadas de un usuario
    /// </summary>
    public async Task<(List<EstadisticaUsuario> estadisticas, long total)> GetPagedByUsuarioAsync(
        Guid idUsuario,
        int page,
        int pageSize)
    {
        var filter = Builders<EstadisticaUsuario>.Filter.Eq(e => e.IdUsuario, idUsuario);
        var total = await _collection.CountDocumentsAsync(filter);
        
        var estadisticas = await _collection
            .Find(filter)
            .SortByDescending(e => e.Fecha)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (estadisticas, total);
    }

    /// <summary>
    /// Inserta una nueva estadística
    /// </summary>
    public async Task InsertAsync(EstadisticaUsuario estadistica)
    {
        await _collection.InsertOneAsync(estadistica);
    }

    /// <summary>
    /// Inserta múltiples estadísticas de forma eficiente (bulk insert)
    /// </summary>
    public async Task InsertManyAsync(IEnumerable<EstadisticaUsuario> estadisticas)
    {
        var estadisticasList = estadisticas.ToList();
        if (estadisticasList.Any())
        {
            await _collection.InsertManyAsync(estadisticasList);
        }
    }

    /// <summary>
    /// Actualiza una estadística existente
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, EstadisticaUsuario estadistica)
    {
        estadistica.Id = id;
        var result = await _collection.ReplaceOneAsync(e => e.Id == id, estadistica);
        return result.ModifiedCount > 0 || result.MatchedCount > 0;
    }

    /// <summary>
    /// Elimina una estadística por su ID
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(e => e.Id == id);
        return result.DeletedCount > 0;
    }
}

