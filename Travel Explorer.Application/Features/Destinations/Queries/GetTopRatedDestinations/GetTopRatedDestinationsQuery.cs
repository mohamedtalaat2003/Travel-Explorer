
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetTopRatedDestinations
{

    
    
    
    public record GetTopRatedDestinationsQuery(int Count = 6) : IRequest<IReadOnlyList<DestinationDto>>;

}
