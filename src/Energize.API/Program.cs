using Energize.API;
using Energize.API.Middlewares.Logging;
using Energize.API.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

services.AddApplicationServices();
services.AddGrpcServices();
services.AddSwaggerServices();

var app = builder.Build();

// Use the middleware extension method
app.UseLoggingMiddleware();

var swaggerOptions =
    builder.Configuration.GetRequiredSection("Swagger").Get<SwaggerOptions>()
    ?? throw new OptionsValidationException("Swagger", typeof(SwaggerOptions), Array.Empty<string>());

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(swaggerOptions.Endpoint, $"{swaggerOptions.Title} {swaggerOptions.Version}");
    c.RoutePrefix = swaggerOptions.RoutePrefix;
});

app.MapGrpcService<ProductionPlanCalculatorService>();

var grpcOptions =
    builder.Configuration.GetSection("Grpc").Get<GrpcOptions>()
    ?? throw new OptionsValidationException("Grpc", typeof(GrpcOptions), Array.Empty<string>());

app.MapGet("/", () => grpcOptions.CommunicationMessage);

app.Run();