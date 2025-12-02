using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

public class ActividadUsuario
{
    [BsonId] public Guid Id { get; set; }
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