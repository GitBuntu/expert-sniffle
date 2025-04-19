namespace dbapi.Services;

public interface IDatabaseService
{
    Task<IEnumerable<string>> GetDatabasesAsync();
    Task<IEnumerable<string>> GetSchemasAsync(string databaseName);
    Task<IEnumerable<string>> GetTablesAsync(string databaseName, string schemaName);
    Task<IEnumerable<string>> GetColumnsAsync(string databaseName, string schemaName, string tableName);
}