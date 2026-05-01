using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Destinations
{
    public class CreateDestinationDto
    {
        [Required(ErrorMessage = "Destination name is required")]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(250)]
        public string Location { get; set; }

        [Range(0, 999999.99)]
        public decimal PricePerNight { get; set; }

        [StringLength(500)]
        public string? ThumbnailUrl { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        [Required]
        public int CategoryId { get; set; }
    }
}
