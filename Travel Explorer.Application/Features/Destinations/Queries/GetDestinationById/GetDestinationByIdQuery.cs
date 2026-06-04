
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById
{

    
    
    
    public record GetDestinationByIdQuery(int Id) : IRequest<DestinationDto?>;

}
