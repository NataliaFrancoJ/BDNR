using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Repositories;

namespace ObligatorioBDNR.Pages.Auth;

public class SetAuthModel : PageModel
{
    private readonly UsuarioRepository _usuarioRepository;

    public SetAuthModel(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IActionResult> OnGetAsync(string userId, string returnUrl = "/")
    {
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
        {
            return RedirectToPage("/Login");
        }

        // Verificar que el usuario existe
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return RedirectToPage("/Login");
        }

        // Establecer cookies (esto funciona porque es una Razor Page, no Blazor)
        var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        };

        Response.Cookies.Append("UserId", usuario.Id.ToString(), cookieOptions);
        Response.Cookies.Append("Username", usuario.Username, cookieOptions);

        // También establecer en sesión
        HttpContext.Session.SetString("UserId", usuario.Id.ToString());
        HttpContext.Session.SetString("Username", usuario.Username);

        // Redirigir a la URL de retorno o a la página principal
        return Redirect(returnUrl);
    }
}

