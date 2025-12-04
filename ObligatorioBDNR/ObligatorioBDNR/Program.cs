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
        
        var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"] 
            ?? "mongodb://localhost:27017";
        var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"] 
            ?? "Duolingo";
        
        builder.Services.AddSingleton<MongoContext>(sp => 
            new MongoContext(mongoConnectionString, mongoDatabaseName));
        
            builder.Services.AddScoped<UsuarioRepository>();
            builder.Services.AddScoped<LogroRepository>();
            builder.Services.AddScoped<ActividadUsuarioRepository>();
            builder.Services.AddScoped<EstadisticaUsuarioRepository>();

        builder.Services.AddScoped<ObligatorioBDNR.Services.SeedService>();
        builder.Services.AddScoped<ObligatorioBDNR.Services.AuthService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        });

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();
                if (mongoContext?.Database != null)
                {
                    var validatorService = new ObligatorioBDNR.Services.MongoValidatorService(mongoContext.Database);
                    validatorService.ConfigurarValidadoresAsync().Wait();
                }
            }
            catch
            {
            }
        }
        
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<ObligatorioBDNR.Services.SeedService>();
                
                seedService.SeedAsync().Wait();
                seedService.Seed1000UsuariosAsync().Wait();
            }
        }

        app.Run();
    }
}