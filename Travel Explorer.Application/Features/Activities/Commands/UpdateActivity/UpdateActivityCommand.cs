
namespace Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity
{
    /// <summary>
    /// Updates an existing activity.
    /// </summary>
    public record UpdateActivityCommand(
        [Required]
        int Id,

        [Required]
        [StringLength(150, MinimumLength = 2)]
        string Name,

        [StringLength(1000)]
        string Description,

        [StringLength(200)]
        string Icon,

        List<string> ImageUrls,

        [Required]
        int DestinationId) : IRequest<ActivityDto?>;
}
