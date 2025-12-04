using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ObligatorioBDNR;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuración de MongoDB desde appsettings.json
        var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"] 
            ?? "mongodb://localhost:27017";
        var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"] 
            ?? "Duolingo";

        // Registrar MongoContext como Singleton
        builder.Services.AddSingleton<MongoContext>(sp => 
            new MongoContext(mongoConnectionString, mongoDatabaseName));

        // Registrar Repositorios como Scoped
            builder.Services.AddScoped<UsuarioRepository>();
            builder.Services.AddScoped<LogroRepository>();
            builder.Services.AddScoped<ActividadUsuarioRepository>();
            builder.Services.AddScoped<EstadisticaUsuarioRepository>();

        // Registrar servicios opcionales
        builder.Services.AddScoped<ObligatorioBDNR.Services.SeedService>();
        builder.Services.AddScoped<ObligatorioBDNR.Services.AuthService>();
        builder.Services.AddHttpContextAccessor();

        // Configurar sesiones
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        });

        // Configurar Blazor Server
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        
        // Usar sesiones
        app.UseSession();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorPages();

        // Configurar validadores de MongoDB
        using (var scope = app.Services.CreateScope())
        {
            var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();
            var validatorService = new ObligatorioBDNR.Services.MongoValidatorService(mongoContext.Database);
            try
            {
                validatorService.ConfigurarValidadoresAsync().Wait();
                Console.WriteLine("Validadores de MongoDB configurados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Advertencia: No se pudieron configurar todos los validadores: {ex.Message}");
                // Continuar la ejecución aunque falle la configuración de validadores
            }
        }

        // Opcional: Ejecutar seed en desarrollo
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<ObligatorioBDNR.Services.SeedService>();
                
                // Seed básico (usuarios de ejemplo)
                seedService.SeedAsync().Wait();
                
                // Seed de 1000 usuarios (comentar si no se desea ejecutar)
                Console.WriteLine("\n=== Iniciando generación de 1000 usuarios ===");
                seedService.Seed1000UsuariosAsync().Wait();
                Console.WriteLine("=== Generación de 1000 usuarios completada ===\n");
            }
        }

        app.Run();
    }
}