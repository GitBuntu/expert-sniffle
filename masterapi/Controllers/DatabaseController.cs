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
}