using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Activities
{
    public class UpdateActivityDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Icon { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        [Required]
        public int DestinationId { get; set; }
    }
}
