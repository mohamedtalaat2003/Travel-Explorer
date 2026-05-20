using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations
{
    /// <summary>
    /// Returns a paginated, optionally filtered list of destinations.
    /// </summary>
    public record GetAllDestinationsQuery(DestinationSpecParams Params) : IRequest<PaginatedResult<DestinationDto>>;
}
