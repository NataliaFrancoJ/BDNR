using Domain;
using MongoDB.Driver;

namespace DataAccess;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("ObligatorioBDRN");
    }

    public IMongoCollection<Usuario> Usuarios 
        => _database.GetCollection<Usuario>("usuarios");

    public IMongoCollection<ActividadUsuario> Actividades
        => _database.GetCollection<ActividadUsuario>("actividad_usuario");

    public IMongoCollection<LogroDefinicion> LogrosDefinicion
        => _database.GetCollection<LogroDefinicion>("logros_definicion");
}