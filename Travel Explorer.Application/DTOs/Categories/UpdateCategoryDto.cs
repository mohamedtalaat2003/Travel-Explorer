using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? IconUrl { get; set; }
    }
}
