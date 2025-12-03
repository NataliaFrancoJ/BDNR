using ObligatorioBDNR.Data.Neo4j;

namespace ObligatorioBDNR.Services.Neo4j;

public class DataSeederService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSeederService> _logger;

    public DataSeederService(IServiceProvider serviceProvider, ILogger<DataSeederService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var recomendacionService = scope.ServiceProvider.GetRequiredService<INeo4jRecomendacionService>();

            _logger.LogInformation("Iniciando carga de datos de ejemplo en Neo4j...");

            await recomendacionService.CargarDatosEjemploAsync();

            _logger.LogInformation("Datos de ejemplo cargados exitosamente en Neo4j.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar datos de ejemplo en Neo4j. Asegúrate de que Neo4j esté ejecutándose.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

