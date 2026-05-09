
namespace Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination
{
    /// <summary>
    /// Creates a new destination. Properties include validation for direct use as a request body.
    /// </summary>
    public record CreateDestinationCommand(
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
        int CategoryId) : IRequest<DestinationDto>;
}
