using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Blogs.Commands.Delete
{
    public record DeleteBlogCommand(int Id) : IRequest<bool>;

}
