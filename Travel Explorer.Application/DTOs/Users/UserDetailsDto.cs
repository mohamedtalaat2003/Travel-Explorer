using System;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Users
{
    public record UserDetailsDto(
        int Id,
        string UserName,
        string? Email,
        Gender? Gender,
        bool IsBlocked,
        string Role,
        string Status,
        DateTime CreatedAt
    );
}
