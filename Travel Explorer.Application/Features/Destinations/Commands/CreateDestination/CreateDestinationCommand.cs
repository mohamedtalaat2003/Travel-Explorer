
namespace Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination
{
    /// <summary>
    /// Creates a new destination. Properties include validation for direct use as a request body.
    /// </summary>
    public record CreateDestinationCommand(
        string Name,
        string Description,
        string Location,
        decimal PricePerNight,
        List<string> ImageUrls,
        int CategoryId) : IRequest<DestinationDto>;
}
