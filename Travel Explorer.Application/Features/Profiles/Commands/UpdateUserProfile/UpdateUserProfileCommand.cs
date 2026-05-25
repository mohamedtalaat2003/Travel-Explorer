using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        string FullName,
        string Email,
        string PhoneNumber,
        string PassportNumber,
        string? Bio,
        string? AvatarUrl,
        string? Country,
        DateTime? DateOfBirth) : IRequest<UserProfileDto?>;
}
