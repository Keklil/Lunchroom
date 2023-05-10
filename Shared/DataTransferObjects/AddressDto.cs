namespace Shared.DataTransferObjects;

public record AddressDto(
    string Country,
    string State,
    string City,
    string Street,
    string Number,
    string? Office);