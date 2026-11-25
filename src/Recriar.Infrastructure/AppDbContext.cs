using MySqlConnector;

namespace Recriar.Infrastructure.Persistence.Context;

public class AppDbContext
{
    private string _connectionString;

    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public MySqlConnection CreateWriteConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public MySqlConnection CreateReadConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
