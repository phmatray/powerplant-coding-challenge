namespace Energize.API.Core.Options;

public record SwaggerOptions
{
    public required string Title { get; set; }
    public required string Version { get; set; }
    public required string Endpoint { get; set; }
    public required string RoutePrefix { get; set; }
}