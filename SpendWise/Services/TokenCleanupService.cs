using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class TokenCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _services;

    public TokenCleanupService(IServiceProvider services)
    {
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
       _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

           var expiredTokens = context.Tokens
                .Where(t => t.FechaExpiracion < DateTime.UtcNow)
                .ToList();

         if (expiredTokens.Any())
            {
                context.Tokens.RemoveRange(expiredTokens);
                context.SaveChanges();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
       _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}