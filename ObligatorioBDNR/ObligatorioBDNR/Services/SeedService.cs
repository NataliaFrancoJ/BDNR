using DataAccess;
using DataAccess.Repositories;
using Domain;

namespace ObligatorioBDNR.Services;

public class SeedService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly LogroRepository _logroRepository;
    private readonly ActividadUsuarioRepository _actividadRepository;
    private readonly EstadisticaUsuarioRepository _estadisticaRepository;

    public SeedService(
        UsuarioRepository usuarioRepository,
        LogroRepository logroRepository,
        ActividadUsuarioRepository actividadRepository,
        EstadisticaUsuarioRepository estadisticaRepository)
    {
        _usuarioRepository = usuarioRepository;
        _logroRepository = logroRepository;
        _actividadRepository = actividadRepository;
        _estadisticaRepository = estadisticaRepository;
    }

    private List<UnidadCurso> GenerarUnidadesCurso(int totalUnidades, int nivelesPorUnidad, int unidadesCompletadas, int unidadActual, int nivelActual)
    {
        var unidades = new List<UnidadCurso>();
        for (int i = 1; i <= totalUnidades; i++)
        {
            var nivelesCompletados = 0;
            var completada = false;
            
            if (i < unidadActual)
            {
                nivelesCompletados = nivelesPorUnidad;
                completada = true;
            }
            else if (i == unidadActual)
            {
                nivelesCompletados = nivelActual;
                completada = nivelActual >= nivelesPorUnidad;
            }
            
            unidades.Add(new UnidadCurso
            {
                NumeroUnidad = i,
                Nombre = $"Unidad {i}",
                NivelesTotales = nivelesPorUnidad,
                NivelesCompletados = nivelesCompletados,
                Completada = completada
            });
        }
        return unidades;
    }

    public async Task SeedAsync()
    {
        var usuariosExistentes = await _usuarioRepository.GetAllAsync();
        var usuariosBasicosExistentes = usuariosExistentes.Where(u => 
            u.Email == "juan.perez@gmail.com" || 
            u.Email == "maria.garcia@gmail.com" || 
            u.Email == "carlos.rodriguez@gmail.com" ||
            u.Email == "ana.martinez@gmail.com" ||
            u.Email == "luis.fernandez@gmail.com" ||
            u.Email == "sofia.lopez@gmail.com" ||
            u.Email == "pedro.sanchez@gmail.com" ||
            u.Email == "laura.torres@gmail.com" ||
            u.Email == "roberto.diaz@gmail.com" ||
            u.Email == "carmen.ruiz@gmail.com"
        ).ToList();
        
        if (usuariosBasicosExistentes.Count >= 10)
        {
            return;
        }
        
        var logros = await _logroRepository.GetAllAsync();
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
            },
            new LogroDefinicion
            {
                Id = "logro_1000_xp",
                Nombre = "1000 XP",
                Descripcion = "Alcanza 1000 XP totales",
                XpRecompensa = 200
            },
            new LogroDefinicion
            {
                Id = "logro_racha_100",
                Nombre = "Racha de 100 días",
                Descripcion = "Mantén una racha de 100 días consecutivos",
                XpRecompensa = 500
            },
            new LogroDefinicion
            {
                Id = "logro_nivel_25",
                Nombre = "Nivel 25",
                Descripcion = "Alcanza el nivel 25 en cualquier idioma",
                XpRecompensa = 150
            },
            new LogroDefinicion
            {
                Id = "logro_poliglota",
                Nombre = "Políglota",
                Descripcion = "Aprende 5 idiomas diferentes",
                XpRecompensa = 300
            },
            new LogroDefinicion
            {
                Id = "logro_velocidad",
                Nombre = "Velocidad",
                Descripcion = "Completa 10 lecciones en un día",
                XpRecompensa = 50
            },
            new LogroDefinicion
            {
                Id = "logro_consistencia",
                Nombre = "Consistencia",
                Descripcion = "Estudia 30 días seguidos",
                XpRecompensa = 100
            },
            new LogroDefinicion
            {
                Id = "logro_maestro",
                Nombre = "Maestro",
                Descripcion = "Completa un curso completo",
                XpRecompensa = 400
            },
            new LogroDefinicion
            {
                Id = "logro_estrella",
                Nombre = "Estrella",
                Descripcion = "Obtén 5 estrellas en una lección",
                XpRecompensa = 20
            },
            new LogroDefinicion
            {
                Id = "logro_maraton",
                Nombre = "Maratón",
                Descripcion = "Estudia más de 60 minutos en un día",
                XpRecompensa = 75
            }
        };

        foreach (var logro in logrosParaCrear)
        {
            var logroExistente = await _logroRepository.GetByIdAsync(logro.Id);
            if (logroExistente == null)
            {
                await _logroRepository.InsertAsync(logro);
            }
        }

        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "juan.perez@gmail.com",
            Username = "JuanPerez",
            FotoPerfil = "https://imagez.tmz.com/image/b8/1by1/2021/10/25/b823cb5212504585a16a46faa55537db_xl.jpg",
            FechaCreacion = DateTime.UtcNow.AddDays(-30),
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
                CompartirActividad = "Amigos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Inglés",
                NivelesCompletados = 5,
                XpTotal = 0
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Inglés",
                    XpAcumulado = 150,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-25),
                    UnidadActual = 3,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Francés",
                    XpAcumulado = 100,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-20),
                    UnidadActual = 2,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 3)
                },
                new CursoUsuario
                {
                    Idioma = "Español",
                    XpAcumulado = 80,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-15),
                    UnidadActual = 2,
                    NivelActual = 1,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 1)
                }
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
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 5,
                RachaMaxima = 12,
                XpSemanaActual = 180,
                XpSemanaAnterior = 150,
                DiasActivosUltimos30 = 18
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

        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "maria.garcia@gmail.com",
            Username = "MariaGarcia",
            FotoPerfil = "https://www.clarin.com/2019/10/23/rBsOy-7E_720x0__1.jpg",
            FechaCreacion = DateTime.UtcNow.AddDays(-15),
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
                PermitirMensajes = false,
                CompartirActividad = "Nadie"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Español",
                NivelesCompletados = 10,
                XpTotal = 0 // Se calculará automáticamente
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Español",
                    XpAcumulado = 300,
                    UnidadesCompletadas = 4,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-14),
                    UnidadActual = 5,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 4, 5, 4)
                },
                new CursoUsuario
                {
                    Idioma = "Portugués",
                    XpAcumulado = 200,
                    UnidadesCompletadas = 3,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-12),
                    UnidadActual = 4,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 3, 4, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Italiano",
                    XpAcumulado = 150,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-10),
                    UnidadActual = 3,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 3)
                }
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
            Amigos = new List<Amigo>(),
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

        var usuario3 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "carlos.rodriguez@gmail.com",
            Username = "CarlosRodriguez",
            FotoPerfil = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/74/Rolling_Stones_04.jpg/250px-Rolling_Stones_04.jpg",
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
                XpTotal = 0 // Se calculará automáticamente
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Francés",
                    XpAcumulado = 250,
                    UnidadesCompletadas = 3,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-18),
                    UnidadActual = 4,
                    NivelActual = 5,
                    Unidades = GenerarUnidadesCurso(10, 5, 3, 4, 5)
                },
                new CursoUsuario
                {
                    Idioma = "Alemán",
                    XpAcumulado = 170,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-15),
                    UnidadActual = 3,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 4)
                },
                new CursoUsuario
                {
                    Idioma = "Inglés",
                    XpAcumulado = 120,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-12),
                    UnidadActual = 2,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 2)
                }
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
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 8,
                RachaMaxima = 15,
                XpSemanaActual = 220,
                XpSemanaAnterior = 190,
                DiasActivosUltimos30 = 20
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
                XpTotal = 0 
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Italiano",
                    XpAcumulado = 100,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-9),
                    UnidadActual = 2,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Español",
                    XpAcumulado = 50,
                    UnidadesCompletadas = 0,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-7),
                    UnidadActual = 1,
                    NivelActual = 1,
                    Unidades = GenerarUnidadesCurso(10, 5, 0, 1, 1)
                },
                new CursoUsuario
                {
                    Idioma = "Francés",
                    XpAcumulado = 75,
                    UnidadesCompletadas = 0,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-5),
                    UnidadActual = 1,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 0, 1, 3)
                }
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
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-5),
                FechaFin = DateTime.UtcNow.AddDays(25),
                AutoRenovacion = true
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 3,
                RachaMaxima = 8,
                XpSemanaActual = 120,
                XpSemanaAnterior = 95,
                DiasActivosUltimos30 = 12
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
                XpTotal = 0 
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Portugués",
                    XpAcumulado = 400,
                    UnidadesCompletadas = 5,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-4),
                    UnidadActual = 6,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 5, 6, 3)
                },
                new CursoUsuario
                {
                    Idioma = "Inglés",
                    XpAcumulado = 280,
                    UnidadesCompletadas = 4,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-3),
                    UnidadActual = 5,
                    NivelActual = 5,
                    Unidades = GenerarUnidadesCurso(10, 5, 4, 5, 5)
                },
                new CursoUsuario
                {
                    Idioma = "Francés",
                    XpAcumulado = 200,
                    UnidadesCompletadas = 3,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-2),
                    UnidadActual = 4,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 3, 4, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Alemán",
                    XpAcumulado = 150,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-1),
                    UnidadActual = 3,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 4)
                }
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
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 4,
                RachaMaxima = 10,
                XpSemanaActual = 280,
                XpSemanaAnterior = 250,
                DiasActivosUltimos30 = 15
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
                XpTotal = 0 // Se calculará automáticamente
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Alemán",
                    XpAcumulado = 500,
                    UnidadesCompletadas = 7,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-40),
                    UnidadActual = 8,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 7, 8, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Inglés",
                    XpAcumulado = 300,
                    UnidadesCompletadas = 4,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-35),
                    UnidadActual = 5,
                    NivelActual = 5,
                    Unidades = GenerarUnidadesCurso(10, 5, 4, 5, 5)
                },
                new CursoUsuario
                {
                    Idioma = "Francés",
                    XpAcumulado = 150,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-30),
                    UnidadActual = 3,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 3)
                },
                new CursoUsuario
                {
                    Idioma = "Italiano",
                    XpAcumulado = 200,
                    UnidadesCompletadas = 3,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-25),
                    UnidadActual = 4,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 3, 4, 4)
                },
                new CursoUsuario
                {
                    Idioma = "Español",
                    XpAcumulado = 180,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-20),
                    UnidadActual = 3,
                    NivelActual = 5,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 5)
                }
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
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-30),
                FechaFin = DateTime.UtcNow.AddDays(60),
                AutoRenovacion = true
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 30,
                RachaMaxima = 45,
                XpSemanaActual = 500,
                XpSemanaAnterior = 420,
                DiasActivosUltimos30 = 28
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

        CalcularXpTotal(usuario1);
        CalcularXpTotal(usuario2);
        CalcularXpTotal(usuario3);
        CalcularXpTotal(usuario4);
        CalcularXpTotal(usuario5);
        CalcularXpTotal(usuario6);

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
                if (usuarioExistente.Cursos == null || !usuarioExistente.Cursos.Any() || 
                    usuarioExistente.Cursos.Count < usuario.Cursos.Count)
                {
                    usuarioExistente.Cursos = usuario.Cursos;
                    CalcularXpTotal(usuarioExistente);
                    await _usuarioRepository.UpdateAsync(usuarioExistente.Id, usuarioExistente);
                }
                else
                {
                    bool necesitaActualizacion = false;
                    foreach (var cursoExistente in usuarioExistente.Cursos)
                    {
                        if (cursoExistente.Unidades == null || !cursoExistente.Unidades.Any())
                        {
                            var cursoNuevo = usuario.Cursos.FirstOrDefault(c => c.Idioma == cursoExistente.Idioma);
                            if (cursoNuevo != null)
                            {
                                cursoExistente.Unidades = cursoNuevo.Unidades;
                                cursoExistente.UnidadActual = cursoNuevo.UnidadActual;
                                cursoExistente.NivelActual = cursoNuevo.NivelActual;
                            }
                            else
                            {
                                cursoExistente.Unidades = GenerarUnidadesCurso(10, 5, cursoExistente.UnidadesCompletadas, 
                                    cursoExistente.UnidadActual > 0 ? cursoExistente.UnidadActual : 1, 
                                    cursoExistente.NivelActual > 0 ? cursoExistente.NivelActual : 1);
                                if (cursoExistente.UnidadActual == 0) cursoExistente.UnidadActual = 1;
                                if (cursoExistente.NivelActual == 0) cursoExistente.NivelActual = 1;
                            }
                            necesitaActualizacion = true;
                        }
                    }
                    // Si no tiene estadísticas generales, agregarlas
                    if (usuarioExistente.EstadisticasGenerales == null && usuario.EstadisticasGenerales != null)
                    {
                        usuarioExistente.EstadisticasGenerales = usuario.EstadisticasGenerales;
                        necesitaActualizacion = true;
                    }
                    if (necesitaActualizacion)
                    {
                        CalcularXpTotal(usuarioExistente);
                        await _usuarioRepository.UpdateAsync(usuarioExistente.Id, usuarioExistente);
                    }
                }
                usuariosFinales.Add(usuarioExistente);
            }
        }
        
        usuario1 = usuariosFinales[0];
        usuario2 = usuariosFinales[1];
        usuario3 = usuariosFinales[2];
        usuario4 = usuariosFinales[3];
        usuario5 = usuariosFinales[4];
        usuario6 = usuariosFinales[5];

        await _usuarioRepository.LimpiarDuplicadosAmigosTodosAsync();

        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario1.Id, usuario3.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario1.Id, usuario5.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario2.Id, usuario1.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario3.Id, usuario2.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario4.Id, usuario2.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario5.Id, usuario3.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario5.Id, usuario4.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario6.Id, usuario2.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario6.Id, usuario3.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario6.Id, usuario5.Id);

        var actividades = new List<ActividadUsuario>();
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
            var actividadExistente = await _actividadRepository.GetByUsuarioAndDateAsync(actividad.IdUsuario, actividad.Fecha);
            if (actividadExistente == null)
            {
                await _actividadRepository.InsertAsync(actividad);
            }
        }

        // Crear estadísticas de ejemplo para usuarios Plus
        var estadisticas = new List<EstadisticaUsuario>();

        // Estadísticas para usuario2 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            XpTotalDia = 45,
            TiempoEstudioMinutos = 20,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_5",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Español", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Portugués", Xp = 15, Lecciones = 1 }
            }
        });

        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            XpTotalDia = 60,
            TiempoEstudioMinutos = 30,
            LeccionesCompletadas = 4,
            UnidadMasTrabajada = "unidad_4",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Español", Xp = 40, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Italiano", Xp = 20, Lecciones = 2 }
            }
        });

        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.Date,
            XpTotalDia = 55,
            TiempoEstudioMinutos = 25,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_5",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Portugués", Xp = 35, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Español", Xp = 20, Lecciones = 1 }
            }
        });

        // Estadísticas para usuario4 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            XpTotalDia = 30,
            TiempoEstudioMinutos = 15,
            LeccionesCompletadas = 2,
            UnidadMasTrabajada = "unidad_2",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Italiano", Xp = 20, Lecciones = 1 },
                new DetallePorCurso { Idioma = "Español", Xp = 10, Lecciones = 1 }
            }
        });

        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.Date,
            XpTotalDia = 40,
            TiempoEstudioMinutos = 20,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_2",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Italiano", Xp = 25, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Francés", Xp = 15, Lecciones = 1 }
            }
        });

        // Estadísticas para usuario6 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            XpTotalDia = 70,
            TiempoEstudioMinutos = 35,
            LeccionesCompletadas = 5,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 40, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 30, Lecciones = 2 }
            }
        });

        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            XpTotalDia = 95,
            TiempoEstudioMinutos = 45,
            LeccionesCompletadas = 6,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 50, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Italiano", Xp = 15, Lecciones = 1 }
            }
        });

        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.Date,
            XpTotalDia = 100,
            TiempoEstudioMinutos = 50,
            LeccionesCompletadas = 7,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 60, Lecciones = 4 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 25, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Francés", Xp = 15, Lecciones = 1 }
            }
        });

        // Insertar todas las estadísticas
        foreach (var estadistica in estadisticas)
        {
            var estadisticaExistente = await _estadisticaRepository.GetByUsuarioAndDateAsync(estadistica.IdUsuario, estadistica.Fecha);
            if (estadisticaExistente == null)
            {
                await _estadisticaRepository.InsertAsync(estadistica);
            }
        }

        // ========== AGREGAR MÁS USUARIOS ==========
        // Usuario 7: Pedro Sánchez
        var usuario7 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "pedro.sanchez@gmail.com",
            Username = "PedroSanchez",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-60),
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
                IdiomaPrincipal = "Japonés",
                NivelesCompletados = 20,
                XpTotal = 0
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Japonés",
                    XpAcumulado = 600,
                    UnidadesCompletadas = 8,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-55),
                    UnidadActual = 9,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 8, 9, 3)
                },
                new CursoUsuario
                {
                    Idioma = "Coreano",
                    XpAcumulado = 400,
                    UnidadesCompletadas = 6,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-50),
                    UnidadActual = 7,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 6, 7, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Chino",
                    XpAcumulado = 300,
                    UnidadesCompletadas = 4,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-45),
                    UnidadActual = 5,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 4, 5, 4)
                }
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario { IdLogro = "logro_primera_leccion", Nombre = "Primera Lección", FechaObtencion = DateTime.UtcNow.AddDays(-55) },
                new LogroUsuario { IdLogro = "logro_100_xp", Nombre = "100 XP", FechaObtencion = DateTime.UtcNow.AddDays(-54) },
                new LogroUsuario { IdLogro = "logro_500_xp", Nombre = "500 XP", FechaObtencion = DateTime.UtcNow.AddDays(-40) },
                new LogroUsuario { IdLogro = "logro_1000_xp", Nombre = "1000 XP", FechaObtencion = DateTime.UtcNow.AddDays(-20) },
                new LogroUsuario { IdLogro = "logro_nivel_10", Nombre = "Nivel 10", FechaObtencion = DateTime.UtcNow.AddDays(-30) },
                new LogroUsuario { IdLogro = "logro_nivel_25", Nombre = "Nivel 25", FechaObtencion = DateTime.UtcNow.AddDays(-10) },
                new LogroUsuario { IdLogro = "logro_poliglota", Nombre = "Políglota", FechaObtencion = DateTime.UtcNow.AddDays(-5) }
            },
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-50),
                FechaFin = DateTime.UtcNow.AddDays(100),
                AutoRenovacion = true
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 45,
                RachaMaxima = 60,
                XpSemanaActual = 600,
                XpSemanaAnterior = 550,
                DiasActivosUltimos30 = 29
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Japonés", "Coreano", "Chino" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 8: Laura Torres
        var usuario8 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "laura.torres@gmail.com",
            Username = "LauraTorres",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-25),
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
                PermitirMensajes = false,
                CompartirActividad = "Amigos"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Ruso",
                NivelesCompletados = 6,
                XpTotal = 0
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Ruso",
                    XpAcumulado = 180,
                    UnidadesCompletadas = 2,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-24),
                    UnidadActual = 3,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 2, 3, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Polaco",
                    XpAcumulado = 120,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-20),
                    UnidadActual = 2,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 3)
                }
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario { IdLogro = "logro_primera_leccion", Nombre = "Primera Lección", FechaObtencion = DateTime.UtcNow.AddDays(-24) },
                new LogroUsuario { IdLogro = "logro_100_xp", Nombre = "100 XP", FechaObtencion = DateTime.UtcNow.AddDays(-22) },
                new LogroUsuario { IdLogro = "logro_nivel_5", Nombre = "Nivel 5", FechaObtencion = DateTime.UtcNow.AddDays(-15) }
            },
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 6,
                RachaMaxima = 10,
                XpSemanaActual = 150,
                XpSemanaAnterior = 120,
                DiasActivosUltimos30 = 18
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Ruso", "Polaco" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = false,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 9: Roberto Díaz
        var usuario9 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "roberto.diaz@gmail.com",
            Username = "RobertoDiaz",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-70),
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
                IdiomaPrincipal = "Árabe",
                NivelesCompletados = 18,
                XpTotal = 0
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Árabe",
                    XpAcumulado = 550,
                    UnidadesCompletadas = 7,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-65),
                    UnidadActual = 8,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 7, 8, 4)
                },
                new CursoUsuario
                {
                    Idioma = "Turco",
                    XpAcumulado = 350,
                    UnidadesCompletadas = 5,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-60),
                    UnidadActual = 6,
                    NivelActual = 2,
                    Unidades = GenerarUnidadesCurso(10, 5, 5, 6, 2)
                },
                new CursoUsuario
                {
                    Idioma = "Hebreo",
                    XpAcumulado = 250,
                    UnidadesCompletadas = 3,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-55),
                    UnidadActual = 4,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 3, 4, 3)
                }
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario { IdLogro = "logro_primera_leccion", Nombre = "Primera Lección", FechaObtencion = DateTime.UtcNow.AddDays(-65) },
                new LogroUsuario { IdLogro = "logro_100_xp", Nombre = "100 XP", FechaObtencion = DateTime.UtcNow.AddDays(-64) },
                new LogroUsuario { IdLogro = "logro_500_xp", Nombre = "500 XP", FechaObtencion = DateTime.UtcNow.AddDays(-45) },
                new LogroUsuario { IdLogro = "logro_nivel_10", Nombre = "Nivel 10", FechaObtencion = DateTime.UtcNow.AddDays(-40) },
                new LogroUsuario { IdLogro = "logro_racha_30", Nombre = "Racha de 30 días", FechaObtencion = DateTime.UtcNow.AddDays(-20) }
            },
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Plus",
                FechaInicio = DateTime.UtcNow.AddDays(-60),
                FechaFin = DateTime.UtcNow.AddDays(120),
                AutoRenovacion = true
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 35,
                RachaMaxima = 50,
                XpSemanaActual = 550,
                XpSemanaAnterior = 500,
                DiasActivosUltimos30 = 28
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Árabe", "Turco", "Hebreo" },
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            }
        };

        // Usuario 10: Carmen Ruiz
        var usuario10 = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = "carmen.ruiz@gmail.com",
            Username = "CarmenRuiz",
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow.AddDays(-18),
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Tiene2FA = false,
                Metodo2FA = ""
            },
            ConfiguracionPrivacidad = new ConfiguracionPrivacidad
            {
                PerfilVisible = true,
                MostrarRacha = false,
                PermitirMensajes = true,
                CompartirActividad = "Nadie"
            },
            ProgresoGeneral = new ProgresoGeneral
            {
                IdiomaPrincipal = "Holandés",
                NivelesCompletados = 4,
                XpTotal = 0
            },
            Cursos = new List<CursoUsuario>
            {
                new CursoUsuario
                {
                    Idioma = "Holandés",
                    XpAcumulado = 140,
                    UnidadesCompletadas = 1,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-17),
                    UnidadActual = 2,
                    NivelActual = 4,
                    Unidades = GenerarUnidadesCurso(10, 5, 1, 2, 4)
                },
                new CursoUsuario
                {
                    Idioma = "Sueco",
                    XpAcumulado = 90,
                    UnidadesCompletadas = 0,
                    FechaInscripcion = DateTime.UtcNow.AddDays(-14),
                    UnidadActual = 1,
                    NivelActual = 3,
                    Unidades = GenerarUnidadesCurso(10, 5, 0, 1, 3)
                }
            },
            Logros = new List<LogroUsuario>
            {
                new LogroUsuario { IdLogro = "logro_primera_leccion", Nombre = "Primera Lección", FechaObtencion = DateTime.UtcNow.AddDays(-17) },
                new LogroUsuario { IdLogro = "logro_100_xp", Nombre = "100 XP", FechaObtencion = DateTime.UtcNow.AddDays(-15) }
            },
            Amigos = new List<Amigo>(),
            Suscripcion = new Suscripcion
            {
                Tipo = "Gratis",
                FechaInicio = null,
                FechaFin = null,
                AutoRenovacion = false
            },
            EstadisticasGenerales = new EstadisticasGenerales
            {
                RachaActual = 4,
                RachaMaxima = 7,
                XpSemanaActual = 110,
                XpSemanaAnterior = 85,
                DiasActivosUltimos30 = 12
            },
            Preferencias = new Preferencias
            {
                IdiomasInteres = new List<string> { "Holandés", "Sueco" },
                Notificaciones = new Notificaciones
                {
                    Email = false,
                    Push = true,
                    RecordatoriosDiarios = false
                }
            }
        };

        // Calcular XP total para los nuevos usuarios
        CalcularXpTotal(usuario7);
        CalcularXpTotal(usuario8);
        CalcularXpTotal(usuario9);
        CalcularXpTotal(usuario10);

        // Insertar o actualizar los nuevos usuarios
        var nuevosUsuarios = new[] { usuario7, usuario8, usuario9, usuario10 };
        var nuevosUsuariosFinales = new List<Usuario>();

        foreach (var usuario in nuevosUsuarios)
        {
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (usuarioExistente == null)
            {
                await _usuarioRepository.InsertAsync(usuario);
                nuevosUsuariosFinales.Add(usuario);
            }
            else
            {
                nuevosUsuariosFinales.Add(usuarioExistente);
            }
        }

        usuario7 = nuevosUsuariosFinales[0];
        usuario8 = nuevosUsuariosFinales[1];
        usuario9 = nuevosUsuariosFinales[2];
        usuario10 = nuevosUsuariosFinales[3];

        // Establecer amistades bidireccionales para los nuevos usuarios
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario7.Id, usuario1.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario7.Id, usuario6.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario8.Id, usuario2.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario8.Id, usuario4.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario9.Id, usuario3.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario9.Id, usuario5.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario10.Id, usuario1.Id);
        await _usuarioRepository.AgregarAmigoBidireccionalAsync(usuario10.Id, usuario8.Id);

        // ========== AGREGAR MÁS ACTIVIDADES ==========
        // Más actividades para usuario1
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(9), IdLeccion = "leccion_4", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(13), IdLeccion = "practica_2", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(10), IdLeccion = "leccion_5", XpGanado = 20 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(16), IdLeccion = "desafio_2", XpGanado = 35 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario1.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(8), IdLeccion = "leccion_6", XpGanado = 25 }
            }
        });

        // Más actividades para usuario2
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(11), IdLeccion = "leccion_4", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(15), IdLeccion = "practica_3", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(10), IdLeccion = "leccion_5", XpGanado = 30 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(14), IdLeccion = "desafio_3", XpGanado = 40 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(9), IdLeccion = "leccion_6", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(12), IdLeccion = "practica_4", XpGanado = 20 }
            }
        });

        // Actividades para usuario7
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-7).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(8), IdLeccion = "leccion_1", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(13), IdLeccion = "practica_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(9), IdLeccion = "leccion_2", XpGanado = 30 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(15), IdLeccion = "desafio_1", XpGanado = 45 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(10), IdLeccion = "leccion_3", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14), IdLeccion = "practica_2", XpGanado = 20 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(18), IdLeccion = "desafio_2", XpGanado = 50 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(8), IdLeccion = "leccion_4", XpGanado = 30 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddHours(12), IdLeccion = "practica_3", XpGanado = 20 }
            }
        });

        // Actividades para usuario8
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario8.Id,
            Fecha = DateTime.UtcNow.AddDays(-6).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-6).AddHours(10), IdLeccion = "leccion_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario8.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(11), IdLeccion = "leccion_2", XpGanado = 20 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(16), IdLeccion = "practica_1", XpGanado = 10 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario8.Id,
            Fecha = DateTime.UtcNow.AddDays(-1).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(9), IdLeccion = "leccion_3", XpGanado = 20 }
            }
        });

        // Actividades para usuario9
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-8).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-8).AddHours(9), IdLeccion = "leccion_1", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-8).AddHours(14), IdLeccion = "practica_1", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(10), IdLeccion = "leccion_2", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(15), IdLeccion = "desafio_1", XpGanado = 35 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(8), IdLeccion = "leccion_3", XpGanado = 25 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(13), IdLeccion = "practica_2", XpGanado = 15 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddHours(9), IdLeccion = "leccion_4", XpGanado = 25 },
                new Accion { Tipo = "Desafío", Timestamp = DateTime.UtcNow.AddHours(16), IdLeccion = "desafio_2", XpGanado = 40 }
            }
        });

        // Actividades para usuario10
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario10.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(11), IdLeccion = "leccion_1", XpGanado = 20 }
            }
        });
        actividades.Add(new ActividadUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario10.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            Acciones = new List<Accion>
            {
                new Accion { Tipo = "Lección Completada", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(10), IdLeccion = "leccion_2", XpGanado = 20 },
                new Accion { Tipo = "Práctica", Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(15), IdLeccion = "practica_1", XpGanado = 10 }
            }
        });

        var nuevasActividades = actividades.Skip(18).ToList(); // Saltamos las 18 actividades originales
        foreach (var actividad in nuevasActividades)
        {
            var actividadExistente = await _actividadRepository.GetByUsuarioAndDateAsync(actividad.IdUsuario, actividad.Fecha);
            if (actividadExistente == null)
            {
                await _actividadRepository.InsertAsync(actividad);
            }
        }

        // ========== AGREGAR MÁS ESTADÍSTICAS ==========
        // Más estadísticas para usuario2 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            XpTotalDia = 50,
            TiempoEstudioMinutos = 25,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_4",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Español", Xp = 35, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Portugués", Xp = 15, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario2.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            XpTotalDia = 70,
            TiempoEstudioMinutos = 35,
            LeccionesCompletadas = 5,
            UnidadMasTrabajada = "unidad_5",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Español", Xp = 45, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Italiano", Xp = 25, Lecciones = 2 }
            }
        });

        // Más estadísticas para usuario4 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.AddDays(-3).Date,
            XpTotalDia = 35,
            TiempoEstudioMinutos = 18,
            LeccionesCompletadas = 2,
            UnidadMasTrabajada = "unidad_2",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Italiano", Xp = 25, Lecciones = 1 },
                new DetallePorCurso { Idioma = "Español", Xp = 10, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario4.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            XpTotalDia = 45,
            TiempoEstudioMinutos = 22,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_2",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Italiano", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Francés", Xp = 15, Lecciones = 1 }
            }
        });

        // Más estadísticas para usuario6 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            XpTotalDia = 80,
            TiempoEstudioMinutos = 40,
            LeccionesCompletadas = 5,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 45, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 35, Lecciones = 2 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            XpTotalDia = 90,
            TiempoEstudioMinutos = 45,
            LeccionesCompletadas = 6,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 50, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Italiano", Xp = 10, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario6.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            XpTotalDia = 85,
            TiempoEstudioMinutos = 42,
            LeccionesCompletadas = 6,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Alemán", Xp = 55, Lecciones = 4 },
                new DetallePorCurso { Idioma = "Inglés", Xp = 20, Lecciones = 1 },
                new DetallePorCurso { Idioma = "Francés", Xp = 10, Lecciones = 1 }
            }
        });

        // Estadísticas para usuario7 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-7).Date,
            XpTotalDia = 50,
            TiempoEstudioMinutos = 25,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_9",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Japonés", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Coreano", Xp = 20, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-4).Date,
            XpTotalDia = 75,
            TiempoEstudioMinutos = 38,
            LeccionesCompletadas = 5,
            UnidadMasTrabajada = "unidad_9",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Japonés", Xp = 45, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Coreano", Xp = 30, Lecciones = 2 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            XpTotalDia = 100,
            TiempoEstudioMinutos = 50,
            LeccionesCompletadas = 7,
            UnidadMasTrabajada = "unidad_9",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Japonés", Xp = 50, Lecciones = 4 },
                new DetallePorCurso { Idioma = "Coreano", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Chino", Xp = 20, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario7.Id,
            Fecha = DateTime.UtcNow.Date,
            XpTotalDia = 50,
            TiempoEstudioMinutos = 25,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_9",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Japonés", Xp = 30, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Coreano", Xp = 20, Lecciones = 1 }
            }
        });

        // Estadísticas para usuario9 (Plus)
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-8).Date,
            XpTotalDia = 40,
            TiempoEstudioMinutos = 20,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Árabe", Xp = 25, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Turco", Xp = 15, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-5).Date,
            XpTotalDia = 60,
            TiempoEstudioMinutos = 30,
            LeccionesCompletadas = 4,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Árabe", Xp = 35, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Turco", Xp = 25, Lecciones = 2 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.AddDays(-2).Date,
            XpTotalDia = 40,
            TiempoEstudioMinutos = 20,
            LeccionesCompletadas = 3,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Árabe", Xp = 25, Lecciones = 2 },
                new DetallePorCurso { Idioma = "Hebreo", Xp = 15, Lecciones = 1 }
            }
        });
        estadisticas.Add(new EstadisticaUsuario
        {
            Id = Guid.NewGuid(),
            IdUsuario = usuario9.Id,
            Fecha = DateTime.UtcNow.Date,
            XpTotalDia = 65,
            TiempoEstudioMinutos = 32,
            LeccionesCompletadas = 4,
            UnidadMasTrabajada = "unidad_8",
            DetallePorCurso = new List<DetallePorCurso>
            {
                new DetallePorCurso { Idioma = "Árabe", Xp = 40, Lecciones = 3 },
                new DetallePorCurso { Idioma = "Turco", Xp = 25, Lecciones = 1 }
            }
        });

        var nuevasEstadisticas = estadisticas.Skip(9).ToList(); // Saltamos las 9 estadísticas originales
        foreach (var estadistica in nuevasEstadisticas)
        {
            var estadisticaExistente = await _estadisticaRepository.GetByUsuarioAndDateAsync(estadistica.IdUsuario, estadistica.Fecha);
            if (estadisticaExistente == null)
            {
                await _estadisticaRepository.InsertAsync(estadistica);
            }
        }
    }

    private void CalcularXpTotal(Usuario usuario)
    {
        if (usuario.Cursos != null && usuario.Cursos.Any())
        {
            usuario.ProgresoGeneral ??= new ProgresoGeneral();
            usuario.ProgresoGeneral.XpTotal = usuario.Cursos.Sum(c => c.XpAcumulado);
        }
        else
        {
            usuario.ProgresoGeneral ??= new ProgresoGeneral();
            usuario.ProgresoGeneral.XpTotal = 0;
        }
    }

    public async Task Seed1000UsuariosAsync()
    {
        var usuariosExistentes = await _usuarioRepository.GetAllAsync();
        var totalUsuarios = usuariosExistentes.Count();
        
        if (totalUsuarios >= 100)
        {
            return;
        }
        
        var random = new Random();
        var idiomas = new[] { "Inglés", "Español", "Francés", "Alemán", "Italiano", "Portugués", "Japonés", "Coreano", "Chino", "Ruso", "Árabe", "Holandés", "Sueco", "Polaco", "Turco" };
        var tiposSuscripcion = new[] { "Gratis", "Plus" };
        var tiposAccion = new[] { "Lección Completada", "Práctica", "Desafío", "Repaso" };
        var logrosDisponibles = await _logroRepository.GetAllAsync();
        
        var usuarios = new List<Usuario>();
        var actividades = new List<ActividadUsuario>();
        var estadisticas = new List<EstadisticaUsuario>();
        
        var usuariosNecesarios = 100 - totalUsuarios;
        
        for (int i = 1; i <= usuariosNecesarios; i++)
        {
            var esPlus = random.Next(100) < 30; // 30% son Plus
            var fechaCreacion = DateTime.UtcNow.AddDays(-random.Next(1, 365));
            var idiomaPrincipal = idiomas[random.Next(idiomas.Length)];
            var numCursos = random.Next(1, 5); // 1-4 cursos por usuario
            
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Email = $"usuario{i}@gmail.com",
                Username = $"Usuario{i}",
                FotoPerfil = "",
                FechaCreacion = fechaCreacion,
                Autenticacion = new Autenticacion
                {
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Tiene2FA = random.Next(100) < 20, // 20% tienen 2FA
                    Metodo2FA = random.Next(100) < 20 ? (random.Next(2) == 0 ? "App" : "SMS") : ""
                },
                ConfiguracionPrivacidad = new ConfiguracionPrivacidad
                {
                    PerfilVisible = random.Next(100) < 80, // 80% visible
                    MostrarRacha = random.Next(100) < 70,
                    PermitirMensajes = random.Next(100) < 60,
                    CompartirActividad = new[] { "todos", "amigos", "nadie" }[random.Next(3)]
                },
                ProgresoGeneral = new ProgresoGeneral
                {
                    IdiomaPrincipal = idiomaPrincipal,
                    NivelesCompletados = random.Next(0, 50),
                    XpTotal = 0
                },
                Cursos = new List<CursoUsuario>(),
                Logros = new List<LogroUsuario>(),
                Amigos = new List<Amigo>(),
                Suscripcion = new Suscripcion
                {
                    Tipo = esPlus ? "Plus" : "Gratis",
                    FechaInicio = esPlus ? fechaCreacion.AddDays(random.Next(30)) : (DateTime?)null,
                    FechaFin = esPlus ? DateTime.UtcNow.AddDays(random.Next(30, 365)) : (DateTime?)null,
                    AutoRenovacion = esPlus && random.Next(100) < 70
                },
                EstadisticasGenerales = new EstadisticasGenerales
                {
                    RachaActual = random.Next(0, 100),
                    RachaMaxima = random.Next(0, 200),
                    XpSemanaActual = random.Next(0, 1000),
                    XpSemanaAnterior = random.Next(0, 1000),
                    DiasActivosUltimos30 = random.Next(0, 30)
                },
                Preferencias = new Preferencias
                {
                    IdiomasInteres = idiomas.OrderBy(x => random.Next()).Take(random.Next(1, 4)).ToList(),
                    Notificaciones = new Notificaciones
                    {
                        Email = random.Next(100) < 70,
                        Push = random.Next(100) < 60,
                        RecordatoriosDiarios = random.Next(100) < 50
                    }
                }
            };

            // Generar cursos
            var idiomasUsados = new HashSet<string> { idiomaPrincipal };
            for (int j = 0; j < numCursos; j++)
            {
                string idiomaCurso;
                do
                {
                    idiomaCurso = idiomas[random.Next(idiomas.Length)];
                } while (idiomasUsados.Contains(idiomaCurso));
                idiomasUsados.Add(idiomaCurso);

                var unidadesCompletadas = random.Next(0, 8);
                var unidadActual = unidadesCompletadas + 1;
                var nivelActual = random.Next(1, 6);
                var xpAcumulado = random.Next(50, 1000);

                usuario.Cursos.Add(new CursoUsuario
                {
                    Idioma = idiomaCurso,
                    XpAcumulado = xpAcumulado,
                    UnidadesCompletadas = unidadesCompletadas,
                    FechaInscripcion = fechaCreacion.AddDays(random.Next(30)),
                    UnidadActual = unidadActual,
                    NivelActual = nivelActual,
                    Unidades = GenerarUnidadesCurso(10, 5, unidadesCompletadas, unidadActual, nivelActual)
                });
            }

            // Generar logros obtenidos (1-5 logros por usuario)
            var numLogros = random.Next(1, 6);
            var logrosObtenidos = logrosDisponibles.OrderBy(x => random.Next()).Take(numLogros).ToList();
            foreach (var logro in logrosObtenidos)
            {
                usuario.Logros.Add(new LogroUsuario
                {
                    IdLogro = logro.Id,
                    Nombre = logro.Nombre,
                    FechaObtencion = fechaCreacion.AddDays(random.Next(1, (DateTime.UtcNow - fechaCreacion).Days))
                });
            }

            CalcularXpTotal(usuario);
            usuarios.Add(usuario);

            var numActividades = random.Next(5, 9);
            var fechasActividades = new HashSet<DateTime>();
            for (int j = 0; j < numActividades; j++)
            {
                DateTime fechaActividad;
                do
                {
                    fechaActividad = DateTime.UtcNow.AddDays(-random.Next(0, 30)).Date;
                } while (fechasActividades.Contains(fechaActividad));
                fechasActividades.Add(fechaActividad);

                var numAcciones = random.Next(1, 4);
                var acciones = new List<Accion>();
                for (int k = 0; k < numAcciones; k++)
                {
                    acciones.Add(new Accion
                    {
                        Tipo = tiposAccion[random.Next(tiposAccion.Length)],
                        Timestamp = fechaActividad.AddHours(random.Next(8, 20)),
                        IdLeccion = $"leccion_{random.Next(1, 100)}",
                        XpGanado = random.Next(10, 50)
                    });
                }

                actividades.Add(new ActividadUsuario
                {
                    Id = Guid.NewGuid(),
                    IdUsuario = usuario.Id,
                    Fecha = fechaActividad,
                    Acciones = acciones
                });
            }

            if (esPlus)
            {
                var numEstadisticas = random.Next(2, 5);
                var fechasEstadisticas = new HashSet<DateTime>();
                for (int j = 0; j < numEstadisticas; j++)
                {
                    DateTime fechaEstadistica;
                    do
                    {
                        fechaEstadistica = DateTime.UtcNow.AddDays(-random.Next(0, 30)).Date;
                    } while (fechasEstadisticas.Contains(fechaEstadistica));
                    fechasEstadisticas.Add(fechaEstadistica);

                    var detallePorCurso = new List<DetallePorCurso>();
                    foreach (var curso in usuario.Cursos.Take(random.Next(1, usuario.Cursos.Count + 1)))
                    {
                        detallePorCurso.Add(new DetallePorCurso
                        {
                            Idioma = curso.Idioma,
                            Xp = random.Next(10, 100),
                            Lecciones = random.Next(1, 5)
                        });
                    }

                    estadisticas.Add(new EstadisticaUsuario
                    {
                        Id = Guid.NewGuid(),
                        IdUsuario = usuario.Id,
                        Fecha = fechaEstadistica,
                        XpTotalDia = random.Next(20, 150),
                        TiempoEstudioMinutos = random.Next(10, 120),
                        LeccionesCompletadas = random.Next(1, 10),
                        UnidadMasTrabajada = $"unidad_{random.Next(1, 10)}",
                        DetallePorCurso = detallePorCurso
                    });
                }
            }

        }

        var usuariosParaInsertar = new List<Usuario>();
        foreach (var usuario in usuarios)
        {
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (usuarioExistente == null)
            {
                usuariosParaInsertar.Add(usuario);
            }
        }
        
        if (usuariosParaInsertar.Any())
        {
            for (int i = 0; i < usuariosParaInsertar.Count; i += 50)
            {
                var lote = usuariosParaInsertar.Skip(i).Take(50).ToList();
                await _usuarioRepository.InsertManyAsync(lote);
            }
        }

        var todosLosUsuarios = await _usuarioRepository.GetAllAsync();
        var usuariosPorEmail = todosLosUsuarios.ToDictionary(u => u.Email, u => u);
        var actividadesParaInsertar = new List<ActividadUsuario>();
        
        foreach (var actividad in actividades)
        {
            var usuarioGenerado = usuarios.FirstOrDefault(u => u.Id == actividad.IdUsuario);
            if (usuarioGenerado != null)
            {
                var usuarioExistente = usuariosPorEmail.GetValueOrDefault(usuarioGenerado.Email);
                if (usuarioExistente != null)
                {
                    var actividadExistente = await _actividadRepository.GetByUsuarioAndDateAsync(usuarioExistente.Id, actividad.Fecha);
                    if (actividadExistente == null)
                    {
                        actividad.IdUsuario = usuarioExistente.Id;
                        actividadesParaInsertar.Add(actividad);
                    }
                }
            }
        }
        
        if (actividadesParaInsertar.Any())
        {
            for (int i = 0; i < actividadesParaInsertar.Count; i += 200)
            {
                var lote = actividadesParaInsertar.Skip(i).Take(200).ToList();
                await _actividadRepository.InsertManyAsync(lote);
            }
        }

        var estadisticasParaInsertar = new List<EstadisticaUsuario>();
        
        foreach (var estadistica in estadisticas)
        {
            var usuarioGenerado = usuarios.FirstOrDefault(u => u.Id == estadistica.IdUsuario);
            if (usuarioGenerado != null)
            {
                var usuarioExistente = usuariosPorEmail.GetValueOrDefault(usuarioGenerado.Email);
                if (usuarioExistente != null)
                {
                    var estadisticaExistente = await _estadisticaRepository.GetByUsuarioAndDateAsync(usuarioExistente.Id, estadistica.Fecha);
                    if (estadisticaExistente == null)
                    {
                        estadistica.IdUsuario = usuarioExistente.Id;
                        estadisticasParaInsertar.Add(estadistica);
                    }
                }
            }
        }
        
        if (estadisticasParaInsertar.Any())
        {
            for (int i = 0; i < estadisticasParaInsertar.Count; i += 200)
            {
                var lote = estadisticasParaInsertar.Skip(i).Take(200).ToList();
                await _estadisticaRepository.InsertManyAsync(lote);
            }
        }

        var usuariosList = todosLosUsuarios.ToList();
        var usuariosIds = usuariosList.Select(u => u.Id).ToList();
        var amistadesEstablecidas = 0;
        var usuariosProcesados = 0;
        var amistadesBidireccionales = 0;
        
        foreach (var usuario in usuariosList)
        {
            var numAmigos = random.Next(2, 6);
            var amigosIds = usuariosIds
                .Where(id => id != usuario.Id)
                .OrderBy(x => random.Next())
                .Take(numAmigos)
                .ToList();

            var necesitaActualizacion = false;
            foreach (var amigoId in amigosIds)
            {
                var amigo = usuariosList.First(u => u.Id == amigoId);
                if (usuario.Amigos == null || !usuario.Amigos.Any(a => a.IdUsuario == amigoId))
                {
                    if (usuario.Amigos == null)
                    {
                        usuario.Amigos = new List<Amigo>();
                    }
                    usuario.Amigos.Add(new Amigo
                    {
                        IdUsuario = amigoId,
                        Username = amigo.Username
                    });
                    amistadesEstablecidas++;
                    necesitaActualizacion = true;
                }
            }

            if (necesitaActualizacion)
            {
                await _usuarioRepository.UpdateAsync(usuario.Id, usuario);
            }

            usuariosProcesados++;
        }

        usuariosProcesados = 0;
        
        foreach (var usuario in usuariosList)
        {
            if (usuario.Amigos != null)
            {
                foreach (var amigo in usuario.Amigos)
                {
                    var amigoUsuario = usuariosList.FirstOrDefault(u => u.Id == amigo.IdUsuario);
                    if (amigoUsuario != null && (amigoUsuario.Amigos == null || !amigoUsuario.Amigos.Any(a => a.IdUsuario == usuario.Id)))
                    {
                        if (amigoUsuario.Amigos == null)
                        {
                            amigoUsuario.Amigos = new List<Amigo>();
                        }
                        amigoUsuario.Amigos.Add(new Amigo
                        {
                            IdUsuario = usuario.Id,
                            Username = usuario.Username
                        });
                        await _usuarioRepository.UpdateAsync(amigoUsuario.Id, amigoUsuario);
                        amistadesBidireccionales++;
                    }
                }
            }

            usuariosProcesados++;
        }

        var actividadesExistentes = await _actividadRepository.GetAllAsync();
        var estadisticasExistentes = await _estadisticaRepository.GetAllAsync();
        var logrosExistentes = await _logroRepository.GetAllAsync();
        
        Console.WriteLine($"\n=== RESUMEN TOTAL DE DOCUMENTOS ===");
        Console.WriteLine($"- Logros: {logrosExistentes.Count()}");
        Console.WriteLine($"- Usuarios: {usuariosList.Count()}");
        Console.WriteLine($"- Actividades: {actividadesExistentes.Count()}");
        Console.WriteLine($"- Estadísticas: {estadisticasExistentes.Count()}");
        Console.WriteLine($"- TOTAL: {logrosExistentes.Count() + usuariosList.Count() + actividadesExistentes.Count() + estadisticasExistentes.Count()} documentos");
        Console.WriteLine($"=====================================\n");
    }
}

