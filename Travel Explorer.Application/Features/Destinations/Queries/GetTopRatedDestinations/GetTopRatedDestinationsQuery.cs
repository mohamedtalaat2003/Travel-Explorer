
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetTopRatedDestinations
{

    /// <summary>
    /// Returns the top-rated destinations ordered by AverageRating descending.
    /// </summary>
    public record GetTopRatedDestinationsQuery(int Count = 6) : IRequest<IReadOnlyList<DestinationDto>>;

}
