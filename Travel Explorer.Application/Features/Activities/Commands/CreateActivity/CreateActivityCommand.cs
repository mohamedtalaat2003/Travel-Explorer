
namespace Travel_Explorer.Application.Features.Activities.Commands.CreateActivity
{
    /// <summary>
    /// Creates a new activity linked to a destination.
    /// </summary>
    public record CreateActivityCommand(
        [Required]
        [StringLength(150, MinimumLength = 2)]
        string Name,

        [StringLength(1000)]
        string Description,

        [StringLength(200)]
        string Icon,

        List<string> ImageUrls,

        [Required]
        int DestinationId) : IRequest<ActivityDto>;
}
