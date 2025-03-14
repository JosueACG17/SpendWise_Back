using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ErrorLogService
{
    private readonly ErrorLogRepository _repository;

    public ErrorLogService(ErrorLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ErrorLogs>> GetAllErrorsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task CreateErrorAsync(string mensajeError, string enlaceError)
    {
        var errorLog = new ErrorLogs
        {
            Mensaje_error = mensajeError,
            Enlace_error = enlaceError,
            Fecha_error = DateTime.UtcNow
        };

        await _repository.CreateAsync(errorLog);
    }
}