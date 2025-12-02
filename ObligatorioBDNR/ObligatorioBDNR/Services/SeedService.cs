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
        // Verificar si ya hay logros, si no, crearlos
        var logros = await _logroRepository.GetAllAsync();

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
            },
            new LogroDefinicion
            {
                Id = "logro_racha_30",
                Nombre = "Racha de 30 días",
                Descripcion = "Mantén una racha de 30 días consecutivos",
                XpRecompensa = 200
            },
            new LogroDefinicion
            {
                Id = "logro_500_xp",
                Nombre = "500 XP",
                Descripcion = "Alcanza 500 XP totales",
                XpRecompensa = 100
            },
            new LogroDefinicion
            {
                Id = "logro_nivel_10",
                Nombre = "Nivel 10",
                Descripcion = "Alcanza el nivel 10 en cualquier idioma",
                XpRecompensa = 75
            },
            new LogroDefinicion
            {
                Id = "logro_perfecto",
                Nombre = "Perfecto",
                Descripcion = "Completa una lección sin errores",
                XpRecompensa = 15
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
            Email = "juan.perez@gmail.com",
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
            Email = "maria.garcia@gmail.com",
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

        // Usuario 3: Carlos Rodríguez
        var usuario3 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "carlos.rodriguez@gmail.com",
            Username = "CarlosRodriguez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-20),
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = false,
                Metodo2FA = ""
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = true,
                PermitirMensajes = true,
                CompartirActividad = "Todos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Francés",
                NivelesCompletados = 8,
                XpTotal = 420
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-19)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-18)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_5",
                    Nombre = "Nivel 5",
                    FechaObtencion = DateTime.UtcNow.AddDays(-15)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_500_xp",
                    Nombre = "500 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-5)
                }
            },
            Amigos = new List<Amigo>
            {
                new Amigo
                {
                    IdUsuario = usuario1.Id,
                    Username = usuario1.Username
                },
                new Amigo
                {
                    IdUsuario = usuario2.Id,
                    Username = usuario2.Username
                }
            },
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Francés", "Alemán" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = false
                }
            }
        };

        // Usuario 4: Ana Martínez
        var usuario4 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "ana.martinez@gmail.com",
            Username = "AnaMartinez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-10),
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = true,
                Metodo2FA = "SMS"
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = false,
                PermitirMensajes = true,
                CompartirActividad = "Amigos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Italiano",
                NivelesCompletados = 3,
                XpTotal = 150
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-9)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-7)
                }
            },
            Amigos = new List<Amigo>
            {
                new Amigo
                {
                    IdUsuario = usuario2.Id,
                    Username = usuario2.Username
                }
            },
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-5),
                FechaFin = DateTime.UtcNow.AddDays(25),
                AutoRenovacion = true
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Italiano", "Español" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 5: Luis Fernández
        var usuario5 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "luis.fernandez@gmail.com",
            Username = "LuisFernandez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-5),
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = false,
                Metodo2FA = ""
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
                IdiomaPrincipal = "Portugués",
                NivelesCompletados = 12,
                XpTotal = 680
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-4)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-3)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_5",
                    Nombre = "Nivel 5",
                    FechaObtencion = DateTime.UtcNow.AddDays(-2)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_500_xp",
                    Nombre = "500 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-1)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_10",
                    Nombre = "Nivel 10",
                    FechaObtencion = DateTime.UtcNow.AddDays(-1)
                }
            },
            Amigos = new List<Amigo>
            {
                new Amigo
                {
                    IdUsuario = usuario1.Id,
                    Username = usuario1.Username
                },
                new Amigo
                {
                    IdUsuario = usuario3.Id,
                    Username = usuario3.Username
                },
                new Amigo
                {
                    IdUsuario = usuario4.Id,
                    Username = usuario4.Username
                }
            },
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Portugués", "Inglés" },
                Notificaciones = new Notificaciones
                {
                    Email = false,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 6: Sofía López
        var usuario6 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "sofia.lopez@gmail.com",
            Username = "SofiaLopez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-45),
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = true,
                Metodo2FA = "App"
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = true,
                PermitirMensajes = true,
                CompartirActividad = "Todos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Alemán",
                NivelesCompletados = 15,
                XpTotal = 950
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario
                {
                    IdLogro = "logro_primera_leccion",
                    Nombre = "Primera Lección",
                    FechaObtencion = DateTime.UtcNow.AddDays(-44)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_100_xp",
                    Nombre = "100 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-40)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_racha_7",
                    Nombre = "Racha de 7 días",
                    FechaObtencion = DateTime.UtcNow.AddDays(-30)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_5",
                    Nombre = "Nivel 5",
                    FechaObtencion = DateTime.UtcNow.AddDays(-35)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_500_xp",
                    Nombre = "500 XP",
                    FechaObtencion = DateTime.UtcNow.AddDays(-20)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_nivel_10",
                    Nombre = "Nivel 10",
                    FechaObtencion = DateTime.UtcNow.AddDays(-10)
                },
                new LogroUsuario
                {
                    IdLogro = "logro_racha_30",
                    Nombre = "Racha de 30 días",
                    FechaObtencion = DateTime.UtcNow.AddDays(-5)
                }
            },
            Amigos = new List<Amigo>
            {
                new Amigo
                {
                    IdUsuario = usuario2.Id,
                    Username = usuario2.Username
                },
                new Amigo
                {
                    IdUsuario = usuario3.Id,
                    Username = usuario3.Username
                },
                new Amigo
                {
                    IdUsuario = usuario5.Id,
                    Username = usuario5.Username
                }
            },
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-30),
                FechaFin = DateTime.UtcNow.AddDays(60),
                AutoRenovacion = true
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Alemán", "Inglés", "Francés" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Actualizar lista de amigos de usuario1 para incluir algunos amigos
        usuario1.Amigos = new List<Amigo>
        {
            new Amigo
            {
                IdUsuario = usuario3.Id,
                Username = usuario3.Username
            },
            new Amigo
            {
                IdUsuario = usuario5.Id,
                Username = usuario5.Username
            }
        };

        // Insertar todos los usuarios solo si no existen, y guardar los IDs reales
        var usuariosParaInsertar = new[] { usuario1, usuario2, usuario3, usuario4, usuario5, usuario6 };
        var usuariosFinales = new List<Usuario>();
        
        foreach (var usuario in usuariosParaInsertar)
        {
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (usuarioExistente == null)
            {
                await _usuarioRepository.InsertAsync(usuario);
                usuariosFinales.Add(usuario);
            }
            else
            {
                usuariosFinales.Add(usuarioExistente);
            }
        }
        
        // Actualizar referencias de IDs para usar los IDs reales
        usuario1 = usuariosFinales[0];
        usuario2 = usuariosFinales[1];
        usuario3 = usuariosFinales[2];
        usuario4 = usuariosFinales[3];
        usuario5 = usuariosFinales[4];
        usuario6 = usuariosFinales[5];

        // Crear actividades de ejemplo para todos los usuarios
        var actividades = new List<ActividadUsuario>();

        // Actividades para usuario1
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(10), IdLeccion = "leccion_1", XpGanado = 20 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14), IdLeccion = "practica_1", XpGanado = 10 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(9), IdLeccion = "leccion_2", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(15), IdLeccion = "desafio_1", XpGanado = 30 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(8), IdLeccion = "leccion_3", XpGanado = 20 }
            }
        });

        // Actividades para usuario2
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(11), IdLeccion = "leccion_1", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(16), IdLeccion = "practica_1", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(10), IdLeccion = "leccion_2", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(13), IdLeccion = "desafio_1", XpGanado = 35 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(9), IdLeccion = "leccion_3", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddHours(12), IdLeccion = "practica_2", XpGanado = 15 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddHours(18), IdLeccion = "desafio_2", XpGanado = 40 }
            }
        });

        // Actividades para usuario3
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario3.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(8), IdLeccion = "leccion_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario3.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(10), IdLeccion = "leccion_2", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14), IdLeccion = "practica_1", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario3.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(9), IdLeccion = "leccion_3", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddHours(15), IdLeccion = "desafio_1", XpGanado = 30 }
            }
        });

        // Actividades para usuario4
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(11), IdLeccion = "leccion_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(10), IdLeccion = "leccion_2", XpGanado = 20 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(16), IdLeccion = "practica_1", XpGanado = 10 }
            }
        });

        // Actividades para usuario5
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario5.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(8), IdLeccion = "leccion_1", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(13), IdLeccion = "practica_1", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario5.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(9), IdLeccion = "leccion_2", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14), IdLeccion = "desafio_1", XpGanado = 35 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario5.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(8), IdLeccion = "leccion_3", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddHours(11), IdLeccion = "practica_2", XpGanado = 15 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddHours(17), IdLeccion = "desafio_2", XpGanado = 40 }
            }
        });

        // Actividades para usuario6
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-7).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(9), IdLeccion = "leccion_1", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(14), IdLeccion = "practica_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(10), IdLeccion = "leccion_2", XpGanado = 30 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(15), IdLeccion = "desafio_1", XpGanado = 40 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(8), IdLeccion = "leccion_3", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(12), IdLeccion = "practica_2", XpGanado = 20 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(16), IdLeccion = "desafio_2", XpGanado = 45 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(9), IdLeccion = "leccion_4", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddHours(13), IdLeccion = "practica_3", XpGanado = 20 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddHours(19), IdLeccion = "desafio_3", XpGanado = 50 }
            }
        });

        // Insertar todas las actividades
        foreach (var actividad in actividades)
        {
            await _actividadRepository.InsertAsync(actividad);
        }
    }
}

