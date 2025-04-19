using MySql.Data.MySqlClient;

namespace dbapi.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetDatabasesAsync()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;User=root;Password=your_password;";

        var databases = new List<string>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = @"
            SELECT SCHEMA_NAME 
            FROM information_schema.SCHEMATA 
            WHERE SCHEMA_NAME NOT IN ('information_schema', 'mysql', 'performance_schema', 'sys')
            ORDER BY SCHEMA_NAME";

        using var command = new MySqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            databases.Add(reader.GetString(0));
        }

        return databases;
    }
}