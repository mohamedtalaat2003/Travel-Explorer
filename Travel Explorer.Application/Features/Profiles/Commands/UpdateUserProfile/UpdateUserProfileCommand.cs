using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public record UpdateUserProfileCommand(
        [Required] string FullName,
        [Required][EmailAddress] string Email,
        [Required][Phone] string PhoneNumber,
        [Required] string PassportNumber,
        string? Bio,
        string? AvatarUrl,
        string? Country,
        DateTime? DateOfBirth) : IRequest<UserProfileDto?>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
