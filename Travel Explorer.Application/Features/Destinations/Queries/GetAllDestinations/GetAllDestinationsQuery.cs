using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations
{
    
    
    
    public record GetAllDestinationsQuery(DestinationSpecParams Params) : IRequest<PaginatedResult<DestinationDto>>;
}
