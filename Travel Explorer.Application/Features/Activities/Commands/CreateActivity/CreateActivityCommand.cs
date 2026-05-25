
namespace Travel_Explorer.Application.Features.Activities.Commands.CreateActivity
{
    /// <summary>
    /// Creates a new activity linked to a destination.
    /// </summary>
    public record CreateActivityCommand(
        string Name,
        string Description,
        string Icon,
        List<string> ImageUrls,
        int DestinationId) : IRequest<ActivityDto>;
}
