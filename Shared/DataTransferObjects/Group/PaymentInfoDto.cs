namespace Shared.DataTransferObjects.Group;

public record PaymentInfoDto
{
    public string Link { get; init; } = null!;
    public string? Description { get; init; }
    public string? Qr { get; init; }
}