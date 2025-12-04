using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain;

public class EstadisticaUsuario
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid IdUsuario { get; set; }

    public DateTime Fecha { get; set; }
    public int XpTotalDia { get; set; }
    public int TiempoEstudioMinutos { get; set; }
    public int LeccionesCompletadas { get; set; }
    public string UnidadMasTrabajada { get; set; } = string.Empty;
    public List<DetallePorCurso> DetallePorCurso { get; set; } = new();
}

public class DetallePorCurso
{
    public string Idioma { get; set; } = string.Empty;
    public int Xp { get; set; }
    public int Lecciones { get; set; }
}

