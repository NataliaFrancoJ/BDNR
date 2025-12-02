using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

public class LogroDefinicion
{
    [BsonId]
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int XpRecompensa { get; set; }
}