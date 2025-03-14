using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ErrorLogRepository
{
    private readonly AppDbContext _context;

    public ErrorLogRepository(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todos los errores ordenados por fecha descendente
    public async Task<List<ErrorLogs>> GetAllAsync()
    {
        return await _context.ErrorLogs
            .OrderByDescending(e => e.Fecha_error)
            .ToListAsync();
    }

    // Crear un nuevo registro de error
    public async Task CreateAsync(ErrorLogs errorLog)
    {
        _context.ErrorLogs.Add(errorLog);
        await _context.SaveChangesAsync();
    }
}