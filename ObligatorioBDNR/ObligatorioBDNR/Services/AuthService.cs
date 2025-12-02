using DataAccess.Repositories;
using Domain;

namespace ObligatorioBDNR.Services;

/// <summary>
/// Servicio de autenticación personalizado
/// </summary>
public class AuthService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(UsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
    {
        _usuarioRepository = usuarioRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Obtiene el usuario actualmente autenticado
    /// </summary>
    public async Task<Usuario?> GetUsuarioActualAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        // Intentar obtener de cookie primero
        var userId = httpContext.Request.Cookies["UserId"];
        if (string.IsNullOrEmpty(userId))
        {
            // Fallback a sesión
            userId = httpContext.Session.GetString("UserId");
        }

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
        {
            return null;
        }

        return await _usuarioRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Obtiene el ID del usuario actualmente autenticado
    /// </summary>
    public Guid? GetUsuarioIdActual()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        // Intentar obtener de cookie primero
        var userId = httpContext.Request.Cookies["UserId"];
        if (string.IsNullOrEmpty(userId))
        {
            // Fallback a sesión
            userId = httpContext.Session.GetString("UserId");
        }

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
        {
            return null;
        }
        return id;
    }

    /// <summary>
    /// Verifica si hay un usuario autenticado
    /// </summary>
    public bool EstaAutenticado()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return false;

        // Intentar obtener de cookie primero
        var userId = httpContext.Request.Cookies["UserId"];
        if (string.IsNullOrEmpty(userId))
        {
            // Fallback a sesión
            userId = httpContext.Session.GetString("UserId");
        }

        return !string.IsNullOrEmpty(userId);
    }

    /// <summary>
    /// Inicia sesión con email y contraseña
    /// </summary>
    public async Task<(bool exito, string mensaje, Usuario? usuario)> LoginAsync(string email, string password)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(email);
        if (usuario == null)
        {
            return (false, "Email o contraseña incorrectos", null);
        }

        if (usuario.Autenticacion == null || string.IsNullOrEmpty(usuario.Autenticacion.PasswordHash))
        {
            return (false, "Email o contraseña incorrectos", null);
        }

        if (!BCrypt.Net.BCrypt.Verify(password, usuario.Autenticacion.PasswordHash))
        {
            return (false, "Email o contraseña incorrectos", null);
        }

        // En Blazor Server, no podemos establecer cookies/sesión después de que la respuesta comenzó
        // Retornamos éxito y redirigimos a una página intermedia que establecerá la autenticación
        return (true, "Inicio de sesión exitoso", usuario);
    }

    /// <summary>
    /// Cierra la sesión del usuario
    /// Nota: En Blazor Server, el logout debe hacerse a través de una página Razor
    /// que redirige a /auth/logout
    /// </summary>
    public void Logout()
    {
        // En Blazor Server no podemos modificar cookies después de que la respuesta comenzó
        // El logout se maneja a través de la página /auth/logout
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            // Intentar limpiar la sesión si es posible
            try
            {
                httpContext.Session.Clear();
            }
            catch
            {
                // Si falla, no es crítico
            }
        }
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    public async Task<(bool exito, string mensaje, Usuario? usuario)> RegistrarAsync(
        string email, 
        string username, 
        string password)
    {
        // Verificar si el email ya existe
        var usuarioExistente = await _usuarioRepository.GetByEmailAsync(email);
        if (usuarioExistente != null)
        {
            return (false, "Ya existe un usuario con este email", null);
        }

        // Verificar si el username ya existe
        usuarioExistente = await _usuarioRepository.GetByUsernameAsync(username);
        if (usuarioExistente != null)
        {
            return (false, "Ya existe un usuario con este username", null);
        }

        // Crear nuevo usuario
        var nuevoUsuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = username,
            FotoPerfil = "",
            FechaCreacion = DateTime.UtcNow,
            Autenticacion = new Autenticacion
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
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
                IdiomaPrincipal = "Español",
                NivelesCompletados = 0,
                XpTotal = 0
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
                IdiomasInteres = new List<string>(),
                Notificaciones = new Notificaciones
                {
                    Email = true,
                    Push = true,
                    RecordatoriosDiarios = true
                }
            },
            Logros = new List<LogroUsuario>(),
            Amigos = new List<Amigo>()
        };

        try
        {
            await _usuarioRepository.InsertAsync(nuevoUsuario);
        }
        catch (Exception ex)
        {
            return (false, $"Error al guardar usuario en la base de datos: {ex.Message}", null);
        }

        // En Blazor Server, no podemos establecer cookies/sesión después de que la respuesta comenzó
        // Retornamos éxito y redirigimos a una página intermedia que establecerá la autenticación

        return (true, "Registro exitoso", nuevoUsuario);
    }
}

