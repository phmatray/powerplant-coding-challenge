namespace Energize.API.Core.Options;

public record GrpcOptions
{
    public required string CommunicationMessage { get; set; }
}