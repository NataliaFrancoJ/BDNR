using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

/// <summary>
/// Repositorio para gestionar operaciones CRUD de actividades de usuarios
/// </summary>
public class ActividadUsuarioRepository
{
    private readonly IMongoCollection<ActividadUsuario> _collection;

    public ActividadUsuarioRepository(MongoContext context)
    {
        _collection = context.Actividades;
    }

    /// <summary>
    /// Obtiene todas las actividades
    /// </summary>
    public async Task<List<ActividadUsuario>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Obtiene una actividad por su ID
    /// </summary>
    public async Task<ActividadUsuario?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene todas las actividades de un usuario
    /// </summary>
    public async Task<List<ActividadUsuario>> GetByUsuarioIdAsync(Guid idUsuario)
    {
        return await _collection
            .Find(a => a.IdUsuario == idUsuario)
            .SortByDescending(a => a.Fecha)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene actividades de un usuario por rango de fechas
    /// </summary>
    public async Task<List<ActividadUsuario>> GetByUsuarioAndDateRangeAsync(
        Guid idUsuario, 
        DateTime fechaInicio, 
        DateTime fechaFin)
    {
        var filter = Builders<ActividadUsuario>.Filter.And(
            Builders<ActividadUsuario>.Filter.Eq(a => a.IdUsuario, idUsuario),
            Builders<ActividadUsuario>.Filter.Gte(a => a.Fecha, fechaInicio.Date),
            Builders<ActividadUsuario>.Filter.Lte(a => a.Fecha, fechaFin.Date)
        );
        return await _collection
            .Find(filter)
            .SortByDescending(a => a.Fecha)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene actividades de un usuario por fecha específica
    /// </summary>
    public async Task<ActividadUsuario?> GetByUsuarioAndDateAsync(Guid idUsuario, DateTime fecha)
    {
        var fechaInicio = fecha.Date;
        var fechaFin = fecha.Date.AddDays(1);
        
        return await _collection
            .Find(a => a.IdUsuario == idUsuario && 
                       a.Fecha >= fechaInicio && 
                       a.Fecha < fechaFin)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene actividades paginadas de un usuario
    /// </summary>
    public async Task<(List<ActividadUsuario> actividades, long total)> GetPagedByUsuarioAsync(
        Guid idUsuario, 
        int page, 
        int pageSize)
    {
        var filter = Builders<ActividadUsuario>.Filter.Eq(a => a.IdUsuario, idUsuario);
        var total = await _collection.CountDocumentsAsync(filter);
        
        var actividades = await _collection
            .Find(filter)
            .SortByDescending(a => a.Fecha)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (actividades, total);
    }

    /// <summary>
    /// Inserta una nueva actividad
    /// </summary>
    public async Task InsertAsync(ActividadUsuario actividad)
    {
        await _collection.InsertOneAsync(actividad);
    }

    /// <summary>
    /// Inserta múltiples actividades de forma eficiente (bulk insert)
    /// </summary>
    public async Task InsertManyAsync(IEnumerable<ActividadUsuario> actividades)
    {
        var actividadesList = actividades.ToList();
        if (actividadesList.Any())
        {
            await _collection.InsertManyAsync(actividadesList);
        }
    }

    /// <summary>
    /// Actualiza una actividad existente
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, ActividadUsuario actividad)
    {
        var result = await _collection.ReplaceOneAsync(a => a.Id == id, actividad);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Elimina una actividad por su ID
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(a => a.Id == id);
        return result.DeletedCount > 0;
    }
}

