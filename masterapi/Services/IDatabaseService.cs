namespace dbapi.Services;

public interface IDatabaseService
{
    Task<IEnumerable<string>> GetDatabasesAsync();
}