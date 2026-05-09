using Travel_Explorer.Application.Features.Activities;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities
{

    /// <summary>
    /// Returns all activities available at a specific destination.
    /// </summary>
    public record GetDestinationActivitiesQuery(int DestinationId) : IRequest<IReadOnlyList<ActivityDto>>;

}
