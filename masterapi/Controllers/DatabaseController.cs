using Microsoft.AspNetCore.Mvc;
using dbapi.Services;

namespace dbapi.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly ILogger<DatabaseController> _logger;
    private readonly IDatabaseService _databaseService;

    public DatabaseController(ILogger<DatabaseController> logger, IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    /// <summary>
    /// Get all databases
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetDatabases()
    {
        try
        {
            var databases = await _databaseService.GetDatabasesAsync();
            return Ok(databases);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting databases");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get all schemas for a given database
    /// </summary>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    [HttpGet("{databaseName}")]
    public async Task<IActionResult> GetSchemas(string databaseName)
    {
        try
        {
            var schemas = await _databaseService.GetSchemasAsync(databaseName);
            return Ok(schemas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schemas for database {DatabaseName}", databaseName);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get all tables for a given database and schema
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="schemaName"></param>
    /// <returns></returns>
    [HttpGet("{databaseName}/{schemaName}")]
    public async Task<IActionResult> GetTables(string databaseName, string schemaName)
    {
        try
        {
            var tables = await _databaseService.GetTablesAsync(databaseName, schemaName);
            return Ok(tables);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tables for database {DatabaseName} and schema {SchemaName}", databaseName, schemaName);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get all columns for a given database, schema and table
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="schemaName"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    [HttpGet("{databaseName}/{schemaName}/{tableName}")]
    public async Task<IActionResult> GetColumns(string databaseName, string schemaName, string tableName)
    {
        try
        {
            var columns = await _databaseService.GetColumnsAsync(databaseName, schemaName, tableName);
            return Ok(columns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting columns for database {DatabaseName}, schema {SchemaName} and table {TableName}", databaseName, schemaName, tableName);
            return StatusCode(500, "Internal server error");
        }
    }
}