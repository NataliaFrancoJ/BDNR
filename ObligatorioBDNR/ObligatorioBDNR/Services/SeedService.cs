using DataAccess;
using DataAccess.Repositories;
using Domain;

namespace ObligatorioBDNR.Services;

/// <summary>
/// Servicio opcional para poblar la base de datos con datos de prueba
/// </summary>
public class SeedService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly LogroRepository _logroRepository;
    private readonly ActividadUsuarioRepository _actividadRepository;

    public SeedService(
        UsuarioRepository usuarioRepository,
        LogroRepository logroRepository,
        ActividadUsuarioRepository actividadRepository)
    {
        _usuarioRepository = usuarioRepository;
        _logroRepository = logroRepository;
        _actividadRepository = actividadRepository;
    }

    /// <summary>
    /// Pobla la base de datos con datos de ejemplo
    /// </summary>
    public async Task SeedAsync()
    {
        // Verificar si ya hay datos
        var usuarios = await _usuarioRepository.GetAllAsync();
        var logros = await _logroRepository.GetAllAsync();
        
        if (usuarios.Any() && logros.Any())
        {
            return; // Ya hay datos, no hacer seed
        }

        // Crear logros de ejemplo solo si no existen
        var logrosParaCrear = new List<LogroDefinicion>
        {
            new LogroDefinicion
            {
                Id = "logro_primera_leccion",
                Nombre = "Primera Lección",
                Descripcion = "Completa tu primera lección",
                XpRecompensa = 10
            },
            new LogroDefinicion
            {
                Id = "logro_racha_7",
                Nombre = "Racha de 7 días",
                Descripcion = "Mantén una racha de 7 días consecutivos",
                XpRecompensa = 50
            },
            new LogroDefinicion
            {
                Id = "logro_100_xp",
                Nombre = "100 XP",
                Descripcion = "Alcanza 100 XP totales",
                XpRecompensa = 25
            },
            new LogroDefinicion
            {
                Id = "logro_nivel_5",
                Nombre = "Nivel 5",
                Descripcion = "Alcanza el nivel 5 en cualquier idioma",
                XpRecompensa = 30
            }
        };

        foreach (var logro in logrosParaCrear)
        {
            // Verificar si el logro ya existe antes de insertarlo
            var logroExistente = await _logroRepository.GetByIdAsync(logro.Id);
            if (logroExistente == null)
            {
                await _logroRepository.InsertAsync(logro);
            }
        }

        // Crear usuarios de ejemplo
        // Usuario 1: Juan Pérez
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "juan.perez@duolingo.com",
            Username = "JuanPerez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-30),
            Autenticacion = new Autenticacion
            {
                // Contraseña: password123
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = false,
                Metodo2FA = ""
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = true,
                PermitirMensajes = true,
                CompartirActividad = "Amigos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Inglés",
                NivelesCompletados = 5,
                XpTotal = 250
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-25)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-20)
                }
            },
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Inglés", "Francés" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 2: María García
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "maria.garcia@duolingo.com",
            Username = "MariaGarcia",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-15),
            Autenticacion = new Autenticacion
            {
                // Contraseña: password123
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = true,
                Metodo2FA = "App"
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = true,
                PermitirMensajes = false,
                CompartirActividad = "Nadie"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Español",
                NivelesCompletados = 10,
                XpTotal = 500
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-14)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-12)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_5",
                    Nombre = "Nivel 5",
                    FechaObtencion = DateTime.UtcNow.AddDays(-10)
                }
            },
            Amigos = new List<Amigo>
            {
                new Amigo
                {
                    IdUsuario = usuario1.Id,
                    Username = usuario1.Username
                }
            },
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-10),
                FechaFin = DateTime.UtcNow.AddDays(20),
                AutoRenovacion = true
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Español", "Portugués", "Italiano" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = false,
                    RecordatoriosDiarios = true
                }
            }
        };

        await _usuarioRepository.InsertAsync(usuario1);
        await _usuarioRepository.InsertAsync(usuario2);

        // Crear actividades de ejemplo
        var actividad1 = new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion
                {
                    Tipo = "Lección Completada",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(10),
                    IdLeccion = "leccion_1",
                    XpGanado = 20
                },
                new Accion
                {
                    Tipo = "Práctica",
                    Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(14),
                    IdLeccion = "practica_1",
                    XpGanado = 10
                }
            }
        };

        var actividad2 = new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion
                {
                    Tipo = "Lección Completada",
                    Timestamp = DateTime.UtcNow.AddHours(9),
                    IdLeccion = "leccion_2",
                    XpGanado = 25
                },
                new Accion
                {
                    Tipo = "Desafío",
                    Timestamp = DateTime.UtcNow.AddHours(12),
                    IdLeccion = "desafio_1",
                    XpGanado = 30
                }
            }
        };

        await _actividadRepository.InsertAsync(actividad1);
        await _actividadRepository.InsertAsync(actividad2);
    }
}

