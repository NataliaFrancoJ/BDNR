using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class LogroRepository
{
    private readonly IMongoCollection<LogroDefinicion> _collection;

    public LogroRepository(MongoContext context)
    {
        _collection = context.Logros;
    }

    public async Task<List<LogroDefinicion>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<LogroDefinicion?> GetByIdAsync(string id)
    {
        return await _collection.Find(l => l.Id == id).FirstOrDefaultAsync();
    }

    public async Task InsertAsync(LogroDefinicion logro)
    {
        await _collection.InsertOneAsync(logro);
    }

    public async Task<bool> UpdateAsync(string id, LogroDefinicion logro)
    {
        var result = await _collection.ReplaceOneAsync(l => l.Id == id, logro);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(l => l.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<LogroDefinicion>> SearchByNameAsync(string searchTerm)
    {
        var filter = Builders<LogroDefinicion>.Filter.Regex(
            l => l.Nombre, 
            new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        return await _collection.Find(filter).ToListAsync();
    }
}

