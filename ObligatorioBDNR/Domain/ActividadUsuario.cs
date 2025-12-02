using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

public class ActividadUsuario
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid IdUsuario { get; set; }
    public DateTime Fecha { get; set; }

    public List<Accion> Acciones { get; set; }
}

public class Accion
    {
        public string Tipo { get; set; }
        public DateTime Timestamp { get; set; }
        public string IdLeccion { get; set; }
        public int XpGanado { get; set; }
    }