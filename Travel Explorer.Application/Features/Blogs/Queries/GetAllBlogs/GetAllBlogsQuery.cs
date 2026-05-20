using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAllBlogs
{
    /// <summary>
    /// Returns a paginated, optionally filtered list of published blogs.
    /// </summary>
    public record GetAllBlogsQuery(BlogSpecParams Params) : IRequest<PaginatedResult<BlogDto>>;
}
