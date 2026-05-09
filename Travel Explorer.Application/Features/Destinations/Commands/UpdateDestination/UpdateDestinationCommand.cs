
namespace Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination
{
    /// <summary>
    /// Updates an existing destination.
    /// Note: The Id can be bound from the route, while other properties come from the body.
    /// </summary>
    public record UpdateDestinationCommand(
        [Required]
        int Id,

        [Required(ErrorMessage = "Destination name is required")]
        [StringLength(200, MinimumLength = 3)]
        string Name,

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000)]
        string Description,

        [Required(ErrorMessage = "Location is required")]
        [StringLength(250)]
        string Location,

        [Range(0, 999999.99)]
        decimal PricePerNight,

        List<string> ImageUrls,

        [Required]
        int CategoryId) : IRequest<DestinationDto?>;
}
