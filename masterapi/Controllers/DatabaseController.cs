using Microsoft.AspNetCore.Mvc;
using dbapi.Services;

namespace dbapi.Controllers;

/// <summary>
/// Controller for managing database operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly ILogger<DatabaseController> _logger;
    private readonly IDatabaseService _databaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseController"/> class.
    /// </summary>
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
    [Route("databases")]
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
    [HttpGet]
    [Route("databases/{databaseName}/schemas")]
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
    [HttpGet]
    [Route("databases/{databaseName}/schemas/{schemaName}/tables")]
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
    [HttpGet]
    [Route("databases/{databaseName}/schemas/{schemaName}/tables/{tableName}/columns")]
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
    /// <summary>
    /// Get a preview (first row) from a specific table
    /// </summary>
    /// <param name="databaseName">The name of the database</param>
    /// <param name="schemaName">The name of the schema</param>
    /// <param name="tableName">The name of the table</param>
    /// <returns>A dictionary of column names and their values</returns>
    [HttpGet]
    [Route("databases/{databaseName}/schemas/{schemaName}/tables/{tableName}/preview")]
    public async Task<IActionResult> GetTablePreview(string databaseName, string schemaName, string tableName)
    {
        try
        {
            var preview = await _databaseService.GetTablePreviewAsync(databaseName, schemaName, tableName);
            return Ok(preview);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting preview for table {TableName} in database {DatabaseName}, schema {SchemaName}",
                tableName, databaseName, schemaName);
            return Problem("An error occurred while processing your request.");
        }
    }
}