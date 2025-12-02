using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ObligatorioBDNR.Pages.Auth;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        // Limpiar cookies
        Response.Cookies.Delete("UserId");
        Response.Cookies.Delete("Username");
        
        // Limpiar sesión
        HttpContext.Session.Clear();

        // Redirigir al login (ruta de Blazor, no Razor Page)
        return Redirect("/login");
    }
}

