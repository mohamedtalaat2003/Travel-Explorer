
using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Queries.GetUserProfile
{
    public record GetUserProfileQuery(int UserId) : IRequest<UserProfileDto?>;
}
