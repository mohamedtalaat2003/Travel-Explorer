using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.Get
{
    public record GetBlogQuery(int Id) : IRequest<BlogDto?>;
}
