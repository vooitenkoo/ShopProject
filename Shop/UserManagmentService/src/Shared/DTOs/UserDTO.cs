using Shared.Enums;

namespace Shared.DTOs;

public record class UserDTO(
    string UserName,
    string UserEmail,
    Role UserRole,
    bool IsActive,
    DateTime CreatedAt
);