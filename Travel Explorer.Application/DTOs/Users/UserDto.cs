using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Users
{
    public record UserDto(
        int Id,
        string UserName,
        string? Email,
        Gender? Gender,
        bool IsBlocked,
        string Status
    );
}
