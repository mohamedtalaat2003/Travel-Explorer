using System.Text.Json.Serialization;
using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        string? FullName,
        string? Bio,
        string? AvatarUrl,
        string? PhoneNumber,
        string? Country,
        DateTime? DateOfBirth,
        string? PassportNumber) : IRequest<UserProfileDto?>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
