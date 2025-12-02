using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

/// <summary>
/// Repositorio para gestionar operaciones CRUD de logros
/// </summary>
public class LogroRepository
{
    private readonly IMongoCollection<LogroDefinicion> _collection;

    public LogroRepository(MongoContext context)
    {
        _collection = context.Logros;
    }

    /// <summary>
    /// Obtiene todos los logros
    /// </summary>
    public async Task<List<LogroDefinicion>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Obtiene un logro por su ID
    /// </summary>
    public async Task<LogroDefinicion?> GetByIdAsync(string id)
    {
        return await _collection.Find(l => l.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserta un nuevo logro
    /// </summary>
    public async Task InsertAsync(LogroDefinicion logro)
    {
        await _collection.InsertOneAsync(logro);
    }

    /// <summary>
    /// Actualiza un logro existente
    /// </summary>
    public async Task<bool> UpdateAsync(string id, LogroDefinicion logro)
    {
        var result = await _collection.ReplaceOneAsync(l => l.Id == id, logro);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Elimina un logro por su ID
    /// </summary>
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(l => l.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Busca logros por nombre
    /// </summary>
    public async Task<List<LogroDefinicion>> SearchByNameAsync(string searchTerm)
    {
        var filter = Builders<LogroDefinicion>.Filter.Regex(
            l => l.Nombre, 
            new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        return await _collection.Find(filter).ToListAsync();
    }
}

