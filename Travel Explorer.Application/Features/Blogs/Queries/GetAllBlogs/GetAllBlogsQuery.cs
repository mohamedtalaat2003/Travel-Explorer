using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAllBlogs
{
    
    
    
    public record GetAllBlogsQuery(BlogSpecParams Params) : IRequest<PaginatedResult<BlogDto>>;
}
