using Shared.Enums;

namespace Shared.DTOs;

public record class RegisterUserDTO (
    string UserName,
    string UserEmail,
    Role UserRole,
    string UserPassword,
    string UserVerifyPassword
);