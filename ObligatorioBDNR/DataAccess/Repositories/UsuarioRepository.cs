using Domain;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class UsuarioRepository
{
    private readonly IMongoCollection<Usuario> _collection;

    public UsuarioRepository(MongoContext context)
    {
        _collection = context.Usuarios;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        var usuario = await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (usuario != null && usuario.Amigos != null && usuario.Amigos.Any())
        {
            var amigosUnicos = usuario.Amigos
                .GroupBy(a => a.IdUsuario)
                .Select(g => g.First())
                .ToList();

            if (amigosUnicos.Count != usuario.Amigos.Count)
            {
                usuario.Amigos = amigosUnicos;
                await UpdateAsync(id, usuario);
            }
        }
        return usuario;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task InsertAsync(Usuario usuario)
    {
        await _collection.InsertOneAsync(usuario);
    }

    public async Task InsertManyAsync(IEnumerable<Usuario> usuarios)
    {
        var usuariosList = usuarios.ToList();
        if (usuariosList.Any())
        {
            await _collection.InsertManyAsync(usuariosList);
        }
    }

    public async Task<bool> UpdateAsync(Guid id, Usuario usuario)
    {
        usuario.Id = id;
        
        if (usuario.Amigos != null && usuario.Amigos.Any())
        {
            usuario.Amigos = usuario.Amigos
                .GroupBy(a => a.IdUsuario)
                .Select(g => g.First())
                .ToList();
        }
        
        var options = new ReplaceOptions { IsUpsert = false };
        var result = await _collection.ReplaceOneAsync(u => u.Id == id, usuario, options);
        
        return result.ModifiedCount > 0 || result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _collection.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Usuario>> GetTopByXpAsync(int limit = 10)
    {
        return await _collection
            .Find(_ => true)
            .SortByDescending(u => u.ProgresoGeneral != null ? u.ProgresoGeneral.XpTotal : 0)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<bool> LimpiarDuplicadosAmigosAsync(Guid idUsuario)
    {
        var usuario = await _collection.Find(u => u.Id == idUsuario).FirstOrDefaultAsync();
        if (usuario == null || usuario.Amigos == null || !usuario.Amigos.Any())
        {
            return false;
        }

        var amigosUnicos = usuario.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();
        
        if (amigosUnicos.Count != usuario.Amigos.Count)
        {
            usuario.Amigos = amigosUnicos;
            usuario.Id = idUsuario;
            var options = new ReplaceOptions { IsUpsert = false };
            var result = await _collection.ReplaceOneAsync(u => u.Id == idUsuario, usuario, options);
            return result.ModifiedCount > 0 || result.MatchedCount > 0;
        }

        return true;
    }

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

    public async Task<bool> AgregarAmigoBidireccionalAsync(Guid idUsuario1, Guid idUsuario2)
    {
        var usuario1 = await GetByIdAsync(idUsuario1);
        var usuario2 = await GetByIdAsync(idUsuario2);

        if (usuario1 == null || usuario2 == null)
        {
            return false;
        }

        usuario1.Amigos ??= new List<Amigo>();
        usuario2.Amigos ??= new List<Amigo>();

        usuario1.Amigos = usuario1.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();
        
        usuario2.Amigos = usuario2.Amigos
            .GroupBy(a => a.IdUsuario)
            .Select(g => g.First())
            .ToList();

        var yaEsAmigo1 = usuario1.Amigos.Any(a => a.IdUsuario == idUsuario2);
        var yaEsAmigo2 = usuario2.Amigos.Any(a => a.IdUsuario == idUsuario1);

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

        var resultado1 = await UpdateAsync(idUsuario1, usuario1);
        var resultado2 = await UpdateAsync(idUsuario2, usuario2);

        return resultado1 && resultado2;
    }

    public async Task<bool> EliminarAmigoBidireccionalAsync(Guid idUsuario1, Guid idUsuario2)
    {
        var usuario1 = await GetByIdAsync(idUsuario1);
        var usuario2 = await GetByIdAsync(idUsuario2);

        if (usuario1 == null || usuario2 == null)
        {
            return false;
        }

        usuario1.Amigos ??= new List<Amigo>();
        usuario2.Amigos ??= new List<Amigo>();

        usuario1.Amigos.RemoveAll(a => a.IdUsuario == idUsuario2);
        usuario2.Amigos.RemoveAll(a => a.IdUsuario == idUsuario1);

        var resultado1 = await UpdateAsync(idUsuario1, usuario1);
        var resultado2 = await UpdateAsync(idUsuario2, usuario2);

        return resultado1 && resultado2;
    }
}

