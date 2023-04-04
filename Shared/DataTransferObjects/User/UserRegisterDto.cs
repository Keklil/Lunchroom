namespace Shared.DataTransferObjects.User;

public record UserRegisterDto(string Email, string Password, RoleDto RoleDto);