using Domain;
using MongoDB.Driver;

namespace DataAccess;

/// <summary>
/// Contexto de MongoDB para la aplicación Duolingo
/// Configura la conexión y expone las colecciones necesarias
/// </summary>
public class MongoContext
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// Obtiene la base de datos de MongoDB
    /// </summary>
    public IMongoDatabase Database => _database;

    /// <summary>
    /// Constructor que inicializa la conexión a MongoDB
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a MongoDB</param>
    /// <param name="databaseName">Nombre de la base de datos</param>
    public MongoContext(string connectionString, string databaseName)
    {
        // Las anotaciones [BsonRepresentation(BsonType.String)] en las clases de dominio
        // se encargan de serializar los GUIDs como strings en MongoDB
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    /// <summary>
    /// Colección de usuarios
    /// </summary>
    public IMongoCollection<Usuario> Usuarios 
        => _database.GetCollection<Usuario>("usuarios");

    /// <summary>
    /// Colección de actividades de usuarios
    /// </summary>
    public IMongoCollection<ActividadUsuario> Actividades
        => _database.GetCollection<ActividadUsuario>("actividad_usuario");

    /// <summary>
    /// Colección de definiciones de logros
    /// </summary>
    public IMongoCollection<LogroDefinicion> Logros
        => _database.GetCollection<LogroDefinicion>("logros_definicion");

    /// <summary>
    /// Colección de estadísticas de usuarios
    /// </summary>
    public IMongoCollection<EstadisticaUsuario> Estadisticas
        => _database.GetCollection<EstadisticaUsuario>("estadisticas_usuario");
}