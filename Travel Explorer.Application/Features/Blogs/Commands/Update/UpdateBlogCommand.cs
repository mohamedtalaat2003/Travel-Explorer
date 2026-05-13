using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Commands.Update
{
    public record UpdateBlogCommand(
    [Required]
    int Id,

    [Required(ErrorMessage = "Blog title is required")]
    [StringLength(250, MinimumLength = 5)]
     string Title,

    [Required(ErrorMessage = "Content is required")]
     string Content,

    [StringLength(500)]
     string ImageUrl,

    string AuthorId ,

    bool IsPublished,

    int? CategoryId
    ) : IRequest<UpdateBlogDto>;
}
