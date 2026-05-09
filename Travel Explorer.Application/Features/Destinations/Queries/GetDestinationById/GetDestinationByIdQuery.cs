
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById
{

    /// <summary>
    /// Returns a single destination by its unique ID.
    /// </summary>
    public record GetDestinationByIdQuery(int Id) : IRequest<DestinationDto?>;

}
