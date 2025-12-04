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
        var usuario = await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (usuario != null && usuario.Amigos != null && usuario.Amigos.Any())
        {
            // Eliminar duplicados al cargar
            var amigosUnicos = usuario.Amigos
                .GroupBy(a => a.IdUsuario)
                .Select(g => g.First())
                .ToList();
            
            // Si había duplicados, guardar la versión limpia
            if (amigosUnicos.Count != usuario.Amigos.Count)
            {
                usuario.Amigos = amigosUnicos;
                // Guardar automáticamente la versión limpia
                await UpdateAsync(id, usuario);
            }
        }
        return usuario;
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
    /// Inserta múltiples usuarios de forma eficiente (bulk insert)
    /// </summary>
    public async Task InsertManyAsync(IEnumerable<Usuario> usuarios)
    {
        var usuariosList = usuarios.ToList();
        if (usuariosList.Any())
        {
            await _collection.InsertManyAsync(usuariosList);
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, Usuario usuario)
    {
        // Asegurarse de que el ID del usuario coincida con el ID buscado
        usuario.Id = id;
        
        // Limpiar duplicados de amigos antes de actualizar
        if (usuario.Amigos != null && usuario.Amigos.Any())
        {
            usuario.Amigos = usuario.Amigos
                .GroupBy(a => a.IdUsuario)
                .Select(g => g.First())
                .ToList();
        }
        
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

    /// <summary>
    /// Elimina duplicados de la lista de amigos de un usuario
    /// </summary>
    public async Task<bool> LimpiarDuplicadosAmigosAsync(Guid idUsuario)
    {
        var usuario = await _collection.Find(u => u.Id == idUsuario).FirstOrDefaultAsync();
        if (usuario == null || usuario.Amigos == null || !usuario.Amigos.Any())
        {
            return false;
        }

        // Agrupar por IdUsuario y mantener solo el primero de cada grupo
        var amigosUnicos = usuario.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();

        // Verificar si hubo cambios
        if (amigosUnicos.Count != usuario.Amigos.Count)
        {
            usuario.Amigos = amigosUnicos;
            usuario.Id = idUsuario; // Asegurar que el ID se mantenga
            var options = new ReplaceOptions { IsUpsert = false };
            var result = await _collection.ReplaceOneAsync(u => u.Id == idUsuario, usuario, options);
            return result.ModifiedCount > 0 || result.MatchedCount > 0;
        }

        return true;
    }

    /// <summary>
    /// Limpia duplicados de amigos en todos los usuarios
    /// </summary>
    public async Task LimpiarDuplicadosAmigosTodosAsync()
    {
        var usuarios = await GetAllAsync();
        foreach (var usuario in usuarios)
        {
            if (usuario.Amigos != null && usuario.Amigos.Any())
            {
                var amigosUnicos = usuario.Amigos
                    .GroupBy(a => a.IdUsuario)
                    .Select(g => g.First())
                    .ToList();

                if (amigosUnicos.Count != usuario.Amigos.Count)
                {
                    usuario.Amigos = amigosUnicos;
                    await UpdateAsync(usuario.Id, usuario);
                }
            }
        }
    }

    /// <summary>
    /// Agrega una amistad bidireccional entre dos usuarios
    /// </summary>
    public async Task<bool> AgregarAmigoBidireccionalAsync(Guid idUsuario1, Guid idUsuario2)
    {
        var usuario1 = await GetByIdAsync(idUsuario1);
        var usuario2 = await GetByIdAsync(idUsuario2);

        if (usuario1 == null || usuario2 == null)
        {
            return false;
        }

        // Inicializar listas de amigos si son null
        usuario1.Amigos ??= new List<Amigo>();
        usuario2.Amigos ??= new List<Amigo>();

        // Eliminar duplicados existentes primero (por si acaso)
        usuario1.Amigos = usuario1.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();
        
        usuario2.Amigos = usuario2.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();

        // Verificar si ya son amigos (evitar duplicados)
        var yaEsAmigo1 = usuario1.Amigos.Any(a => a.IdUsuario == idUsuario2);
        var yaEsAmigo2 = usuario2.Amigos.Any(a => a.IdUsuario == idUsuario1);

        // Agregar amigo en ambas direcciones si no existe
        if (!yaEsAmigo1)
        {
            usuario1.Amigos.Add(new Amigo
            {
                IdUsuario = idUsuario2,
                Username = usuario2.Username
            });
        }

        if (!yaEsAmigo2)
        {
            usuario2.Amigos.Add(new Amigo
            {
                IdUsuario = idUsuario1,
                Username = usuario1.Username
            });
        }

        // Actualizar ambos usuarios
        var resultado1 = await UpdateAsync(idUsuario1, usuario1);
        var resultado2 = await UpdateAsync(idUsuario2, usuario2);

        return resultado1 && resultado2;
    }

    /// <summary>
    /// Elimina una amistad bidireccional entre dos usuarios
    /// </summary>
    public async Task<bool> EliminarAmigoBidireccionalAsync(Guid idUsuario1, Guid idUsuario2)
    {
        var usuario1 = await GetByIdAsync(idUsuario1);
        var usuario2 = await GetByIdAsync(idUsuario2);

        if (usuario1 == null || usuario2 == null)
        {
            return false;
        }

        // Inicializar listas de amigos si son null
        usuario1.Amigos ??= new List<Amigo>();
        usuario2.Amigos ??= new List<Amigo>();

        // Eliminar amigo en ambas direcciones
        usuario1.Amigos.RemoveAll(a => a.IdUsuario == idUsuario2);
        usuario2.Amigos.RemoveAll(a => a.IdUsuario == idUsuario1);

        // Actualizar ambos usuarios
        var resultado1 = await UpdateAsync(idUsuario1, usuario1);
        var resultado2 = await UpdateAsync(idUsuario2, usuario2);

        return resultado1 && resultado2;
    }
}

