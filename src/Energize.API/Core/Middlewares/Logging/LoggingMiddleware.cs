using Grpc.Core;

namespace Energize.API.Core.Middlewares.Logging;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            logger.LogInformation("Request started");
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while processing request.");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error", ex));
        }
        finally
        {
            logger.LogInformation("Request completed");
        }
    }
}