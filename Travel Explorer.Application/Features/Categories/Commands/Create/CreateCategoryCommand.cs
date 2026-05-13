using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories.Commands.Create
{
    public record CreateCategoryCommand
    (
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, MinimumLength = 2)]
    string Name,

    [StringLength(500)]
     string? Description,

    [StringLength(500)]
     string? IconUrl) : IRequest<CategoryDto>;
}
