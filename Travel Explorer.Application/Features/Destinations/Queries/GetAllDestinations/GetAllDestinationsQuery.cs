using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations
{

    /// <summary>
    /// Returns a paginated list of all active destinations.
    /// </summary>
    public record GetAllDestinationsQuery(int PageNumber = 1, int PageSize = 10)
        : IRequest<PaginatedResult<DestinationDto>>;

}
