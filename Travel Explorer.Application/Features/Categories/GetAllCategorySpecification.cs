using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories
{
    public class GetAllCategorySpecification : BaseSpecification<Category>
    {
        public GetAllCategorySpecification(int? pageNumber , int? pageSize) :base()
        {
            AddInclude(c => c.Blogs);
            AddInclude(c => c.Destinations);
            AddOrderBy(c => c.Id);
            if (pageNumber.HasValue && pageSize.HasValue)
                ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }
    }
}
