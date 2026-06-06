
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination
{
    
    
    
    
    public record UpdateDestinationCommand(
        string Name,
        string Description,
        string Location,
        decimal PricePerNight,
        List<string> ImageUrls,
        int CategoryId) : IRequest<DestinationDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
