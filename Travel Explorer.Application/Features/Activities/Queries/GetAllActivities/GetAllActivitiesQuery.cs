
namespace Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities
{

    /// <summary>
    /// Returns all active activities across all destinations.
    /// </summary>
    public record GetAllActivitiesQuery(int ? destinationId =null) : IRequest<IReadOnlyList<ActivityDto>>;

}
