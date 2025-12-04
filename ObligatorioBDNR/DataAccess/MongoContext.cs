using Domain;
using MongoDB.Driver;

namespace DataAccess;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public IMongoDatabase Database => _database;

    public MongoContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Usuario> Usuarios 
        => _database.GetCollection<Usuario>("usuarios");

    public IMongoCollection<ActividadUsuario> Actividades
        => _database.GetCollection<ActividadUsuario>("actividad_usuario");

    public IMongoCollection<LogroDefinicion> Logros
        => _database.GetCollection<LogroDefinicion>("logros_definicion");

    public IMongoCollection<EstadisticaUsuario> Estadisticas
        => _database.GetCollection<EstadisticaUsuario>("estadisticas_usuario");
}
