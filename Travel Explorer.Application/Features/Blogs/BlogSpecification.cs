using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Blogs
{
    public class BlogSpecification : BaseSpecification<Blog>
    {
        public BlogSpecification(int id) :base(b => b.Id == id && !b.IsDeleted && b.IsPublished)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
        }
        public BlogSpecification(int? AuthorId, int? CategoryId) :
            base( b => (!AuthorId.HasValue || b.AuthorId == AuthorId.Value) &&
            (!CategoryId.HasValue || b.CategoryId == CategoryId.Value) &&
            (!b.IsDeleted) && b.IsPublished)
           

        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
        }


    }
}
