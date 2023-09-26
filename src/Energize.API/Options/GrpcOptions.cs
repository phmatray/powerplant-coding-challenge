namespace Energize.API.Options;

public record GrpcOptions
{
    public required string CommunicationMessage { get; set; }
}