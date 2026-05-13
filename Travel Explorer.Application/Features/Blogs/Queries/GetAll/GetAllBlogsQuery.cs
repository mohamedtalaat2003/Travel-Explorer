using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAll
{
    public record GetAllBlogsQuery(int PageNumber = 1, int PageSize = 10)
        : IRequest<PaginatedResult<BlogDto>>;
}
