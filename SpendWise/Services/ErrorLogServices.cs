using Microsoft.EntityFrameworkCore;
using SpendWise.Models;
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
}