using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Commands.CreateBlog
{
    public record CreateBlogCommand(
        string Title,
        string Content,
        string ImageUrl,
        bool IsPublished,
        int? CategoryId
    ) : IRequest<BlogDto>;
}
