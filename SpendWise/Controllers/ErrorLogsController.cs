using Microsoft.AspNetCore.Mvc;
using SpendWise.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ErrorLogsController : ControllerBase
{
    private readonly ErrorLogService _errorLogService;

    public ErrorLogsController(ErrorLogService errorLogService)
    {
        _errorLogService = errorLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllErrorLogs()
    {
        var errores = await _errorLogService.GetAllErrorsAsync();
        return Ok(errores);
    }
}
