using Grpc.Core;

namespace Energize.API.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Request started");
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing request.");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error", ex));
        }
        finally
        {
            _logger.LogInformation("Request completed");
        }
    }
}
