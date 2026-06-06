
namespace Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination
{
    
    
    
    public record CreateDestinationCommand(
        string Name,
        string Description,
        string Location,
        decimal PricePerNight,
        List<string> ImageUrls,
        int CategoryId) : IRequest<DestinationDto>;
}
