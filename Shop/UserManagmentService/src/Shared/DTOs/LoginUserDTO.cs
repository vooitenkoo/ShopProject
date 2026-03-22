namespace Shared.DTOs;

public record class LoginUserDTO(
    string UserName,
    string UserPassword
);