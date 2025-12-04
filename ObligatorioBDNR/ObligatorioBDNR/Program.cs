using ObligatorioBDNR.Data.Neo4j;
using ObligatorioBDNR.Services.Neo4j;

namespace ObligatorioBDNR;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddSingleton<Neo4jContext>();
        builder.Services.AddScoped<INeo4jRecomendacionService, Neo4jRecomendacionService>();

        builder.Services.AddHostedService<DataSeederService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}