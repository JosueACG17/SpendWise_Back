using Microsoft.EntityFrameworkCore;
using SpendWise.Models;

public class ErrorLogRepository
{
    private readonly AppDbContext _context;

    public ErrorLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ErrorLogs>> GetAllAsync()
    {
        return await _context.ErrorLogs
            .OrderByDescending(e => e.Fecha_error)
            .ToListAsync();
    }
}