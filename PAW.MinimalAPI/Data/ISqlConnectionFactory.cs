using Microsoft.Data.SqlClient;

namespace PAW.MinimalAPI.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ProductDB")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:ProductDB en PAW.MinimalAPI.");
    }
    public SqlConnection CreateConnection() => new(_connectionString);
}
