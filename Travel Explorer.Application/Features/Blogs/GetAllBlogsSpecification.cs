using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Blogs
{
    public class GetAllBlogsSpecification : BaseSpecification<Blog>
    {
        public GetAllBlogsSpecification(int? pageNumber = null, int? pageSize = null)
           : base(b=>b.IsPublished)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Category);
            AddOrderByDescending(b => b.CreatedAt);
            if (pageNumber.HasValue && pageSize.HasValue)
                ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }

    }
}
