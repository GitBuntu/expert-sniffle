namespace dbapi.Services;

public interface IDatabaseService
{
    /// <summary>
    /// Retrieves a list of available databases.
    /// </summary>
    /// <returns>A collection of database names.</returns>
    Task<IEnumerable<string>> GetDatabasesAsync();
    /// <summary>
    /// Retrieves a list of schemas for a specified database.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>A collection of schema names.</returns>
    Task<IEnumerable<string>> GetSchemasAsync(string databaseName);
    /// <summary>
    /// Retrieves a list of tables for a specified database and schema.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <returns>A collection of table names.</returns>
    Task<IEnumerable<string>> GetTablesAsync(string databaseName, string schemaName);
    /// <summary>
    /// Retrieves a list of columns for a specified database, schema, and table.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A collection of column names.</returns>
    Task<IEnumerable<string>> GetColumnsAsync(string databaseName, string schemaName, string tableName);
}