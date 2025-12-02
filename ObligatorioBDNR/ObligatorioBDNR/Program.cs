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

        // Registrar servicios opcionales
        builder.Services.AddScoped<ObligatorioBDNR.Services.SeedService>();

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

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorPages();

        // Opcional: Ejecutar seed en desarrollo
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<ObligatorioBDNR.Services.SeedService>();
                seedService.SeedAsync().Wait();
            }
        }

        app.Run();
    }
}