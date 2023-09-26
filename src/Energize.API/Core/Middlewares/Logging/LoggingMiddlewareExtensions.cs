namespace Energize.API.Core.Middlewares.Logging;

public static class LoggingMiddlewareExtensions
{
    public static void UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<LoggingMiddleware>();
    }
}