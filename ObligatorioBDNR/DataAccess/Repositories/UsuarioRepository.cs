using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

/// <summary>
/// Repositorio para gestionar operaciones CRUD de usuarios
/// </summary>
public class UsuarioRepository
{
    private readonly IMongoCollection<Usuario> _collection;

    public UsuarioRepository(MongoContext context)
    {
        _collection = context.Usuarios;
    }

    /// <summary>
    /// Obtiene todos los usuarios
    /// </summary>
    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene un usuario por email
    /// </summary>
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene un usuario por username
    /// </summary>
    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserta un nuevo usuario
    /// </summary>
    public async Task InsertAsync(Usuario usuario)
    {
        await _collection.InsertOneAsync(usuario);
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, Usuario usuario)
    {
        // Asegurarse de que el ID del usuario coincida con el ID buscado
        usuario.Id = id;
        
        // Usar IsUpsert = false para que solo actualice si existe
        var options = new ReplaceOptions { IsUpsert = false };
        var result = await _collection.ReplaceOneAsync(u => u.Id == id, usuario, options);
        
        // Retornar true si se modificó o si se encontró el documento (matched)
        return result.ModifiedCount > 0 || result.MatchedCount > 0;
    }

    /// <summary>
    /// Elimina un usuario por su ID
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Obtiene usuarios con más XP (ranking)
    /// </summary>
    public async Task<List<Usuario>> GetTopByXpAsync(int limit = 10)
    {
        return await _collection
            .Find(_ => true)
            .SortByDescending(u => u.ProgresoGeneral != null ? u.ProgresoGeneral.XpTotal : 0)
            .Limit(limit)
            .ToListAsync();
    }
}

