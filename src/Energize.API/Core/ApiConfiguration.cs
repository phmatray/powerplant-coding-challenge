using Energize.API.Core.Options;
using Microsoft.Extensions.Options;

namespace Energize.API.Core;

internal static class ApiConfiguration
{
    public static void ConfigureSwagger(
        this IApplicationBuilder app,
        WebApplicationBuilder builder)
    {
        var swaggerOptions =
            builder.Configuration.GetRequiredSection("Swagger").Get<SwaggerOptions>()
            ?? throw new OptionsValidationException("Swagger", typeof(SwaggerOptions), Array.Empty<string>());

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(swaggerOptions.Endpoint, $"{swaggerOptions.Title} {swaggerOptions.Version}");
            c.RoutePrefix = swaggerOptions.RoutePrefix;
        });
    }

    public static void ConfigureGrpc(
        this IEndpointRouteBuilder app,
        WebApplicationBuilder builder)
    {
        app.MapGrpcService<ProductionPlanCalculatorService>();

        var grpcOptions =
            builder.Configuration.GetSection("Grpc").Get<GrpcOptions>()
            ?? throw new OptionsValidationException("Grpc", typeof(GrpcOptions), Array.Empty<string>());

        app.MapGet("/", () => grpcOptions.CommunicationMessage);
    }
}