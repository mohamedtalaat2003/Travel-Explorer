namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities
{

    
    
    
    public record GetDestinationActivitiesQuery(int DestinationId) : IRequest<IReadOnlyList<ActivityDto>>;

}
