using Npgsql;

namespace DataAccess;

public class DatabaseConnection
{
    private static readonly string ConnectionString = new NpgsqlConnectionStringBuilder
    {
        Host = "localhost",
        Port = 6432,
        Username = "postgres",
        Password = "postgres",
        SslMode = SslMode.Prefer,
    }.ConnectionString;

    private static NpgsqlConnection _connection;

    public static async Task<NpgsqlConnection> GetCConnectionAsync()
    {
        if (_connection == null)
        {
            _connection = new NpgsqlConnection(ConnectionString);
            await _connection.OpenAsync();
        }

        return _connection;
    }
}