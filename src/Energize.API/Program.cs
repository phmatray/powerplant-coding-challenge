using Energize.API.Core;
using Energize.API.Core.Middlewares.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(8888, o => o.Protocols = HttpProtocols.Http1AndHttp2);
});

services.AddApplicationServices();
services.AddGrpcServices();
services.AddSwaggerServices();

var app = builder.Build();

// Use the middleware extension method
app.UseLoggingMiddleware();

app.ConfigureSwagger(builder);
app.ConfigureGrpc(builder);

app.Run();