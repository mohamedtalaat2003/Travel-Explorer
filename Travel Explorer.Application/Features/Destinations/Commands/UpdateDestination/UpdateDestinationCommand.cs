
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination
{
    /// <summary>
    /// Updates an existing destination.
    /// Note: The Id can be bound from the route, while other properties come from the body.
    /// </summary>
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
