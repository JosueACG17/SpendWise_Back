namespace SpendWise.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ErrorLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Aquí capturas errores como 404, 401, etc.
                if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var errorLogService = scope.ServiceProvider.GetRequiredService<ErrorLogService>();
                        var mensaje = $"Error {context.Response.StatusCode} en {context.Request.Method} {context.Request.Path}";
                        await errorLogService.CreateErrorAsync(mensaje, context.Request.Path);
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura de errores no controlados (excepciones reales)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var errorLogService = scope.ServiceProvider.GetRequiredService<ErrorLogService>();
                    await errorLogService.CreateErrorAsync(ex.Message, context.Request.Path);
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Ocurrió un error inesperado.");
            }
        }
    }

}
