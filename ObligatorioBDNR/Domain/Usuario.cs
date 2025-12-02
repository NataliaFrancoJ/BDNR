using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
namespace Domain;

public class Usuario
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Email { get; set; }
    public string Username { get; set; }
    public string FotoPerfil { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Autenticacion Autenticacion { get; set; }
    public ConfiguracionPrivacidad ConfiguracionPrivacidad { get; set; }
    public ProgresoGeneral ProgresoGeneral { get; set; }

    public List<LogroUsuario> Logros { get; set; }
    public List<Amigo> Amigos { get; set; }

    public Suscripcion Suscripcion { get; set; }
    public Preferencias Preferencias { get; set; }
}

public class Autenticacion
{
    public string PasswordHash { get; set; }
    public bool Tiene2FA { get; set; }
    public string Metodo2FA { get; set; }
}

public class ConfiguracionPrivacidad
{
    public bool PerfilVisible { get; set; }
    public bool MostrarRacha { get; set; }
    public bool PermitirMensajes { get; set; }
    public string CompartirActividad { get; set; }
}

public class ProgresoGeneral
{
    public string IdiomaPrincipal { get; set; }
    public int NivelesCompletados { get; set; }
    public int XpTotal { get; set; }
}

public class LogroUsuario
{
    public string IdLogro { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaObtencion { get; set; }
}

public class Amigo
{
    [BsonRepresentation(BsonType.String)]
    public Guid IdUsuario { get; set; }
    public string Username { get; set; }
}

public class Suscripcion
{
    public string Tipo { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool AutoRenovacion { get; set; }
}

public class Preferencias
{
    public List<string> IdiomasInteres { get; set; }
    public Notificaciones Notificaciones { get; set; }
}

public class Notificaciones
{
    public bool Email { get; set; }
    public bool Push { get; set; }
    public bool RecordatoriosDiarios { get; set; }
}
