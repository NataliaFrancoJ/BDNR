using Neo4j.Driver;
using Microsoft.Extensions.Configuration;

namespace ObligatorioBDNR.Data.Neo4j;

public class Neo4jContext : IDisposable
{
    private readonly IDriver _driver;

    public Neo4jContext(IConfiguration configuration)
    {
        var uri = configuration["Neo4j:Uri"] ?? "bolt://localhost:7687";
        var username = configuration["Neo4j:Username"] ?? "neo4j";
        var password = configuration["Neo4j:Password"] ?? "password";

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
    }

    public IDriver Driver => _driver;

    public void Dispose()
    {
        _driver?.Dispose();
    }
}

