using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetBlogById
{
    public record GetBlogByIdQuery(int Id) : IRequest<BlogDto>;
}
