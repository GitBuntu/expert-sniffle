using MySql.Data.MySqlClient;

namespace dbapi.Services;
/// <summary>
/// Service for interacting with the database.
/// /summary>
public class DatabaseService : IDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="logger">The logger.</param>
    public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    /// <summary>
    /// Retrieves a list of available databases.
    /// </summary>
    /// <returns>A collection of database names.</returns>
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
    /// <summary>
    /// Retrieves a list of schemas for a specified database.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>A collection of schema names.</returns>
    public async Task<IEnumerable<string>> GetSchemasAsync(string databaseName)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;User=root;Password=your_password;";

        var schemas = new List<string>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = @"
        SELECT TABLE_SCHEMA 
        FROM information_schema.TABLES 
        WHERE TABLE_SCHEMA = @DatabaseName
        GROUP BY TABLE_SCHEMA";

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@DatabaseName", databaseName);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            schemas.Add(reader.GetString(0));
        }

        return schemas;
    }
    /// <summary>
    /// Retrieves a list of tables for a specified database and schema.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <returns>A collection of table names.</returns>
    public async Task<IEnumerable<string>> GetTablesAsync(string databaseName, string schemaName)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;User=root;Password=your_password;";

        var tables = new List<string>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = @"
            SELECT TABLE_NAME 
            FROM information_schema.TABLES 
            WHERE TABLE_SCHEMA = @SchemaName
            AND TABLE_NAME NOT LIKE 'sys_%'
            AND TABLE_TYPE = 'BASE TABLE'
            ORDER BY TABLE_NAME";

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SchemaName", schemaName);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }

        return tables;
    }
    /// <summary>
    /// Retrieves a list of columns for a specified database, schema, and table.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A collection of column names.</returns>
    public async Task<IEnumerable<string>> GetColumnsAsync(string databaseName, string schemaName, string tableName)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;User=root;Password=your_password;";

        var columns = new List<string>();

        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = @"
            SELECT COLUMN_NAME 
            FROM information_schema.COLUMNS 
            WHERE TABLE_SCHEMA = @SchemaName
            AND TABLE_NAME = @TableName
            ORDER BY ORDINAL_POSITION";

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SchemaName", schemaName);
        command.Parameters.AddWithValue("@TableName", tableName);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            columns.Add(reader.GetString(0));
        }

        return columns;
    }
}